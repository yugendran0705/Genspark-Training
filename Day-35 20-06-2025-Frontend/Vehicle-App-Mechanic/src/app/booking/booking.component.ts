import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { BookingService } from '../services/booking.service';
import { ImageService } from '../services/image.service';
import { InvoiceService } from '../services/invoice.service';

@Component({
  selector: 'app-booking',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css'
})
export class BookingComponent implements OnInit{
  errorMessage: string = '';
  booking: any = null;
  bookingId: number = 0;
  vehicleId: number = 0;
  base64Image: string = "";
  images: any[] = [];
  currentIndex = 0;
  intervalId: any;
  isInvoiceModalOpen = false;
  invoiceForm!: FormGroup;

  constructor(
    private bookingService: BookingService,
    private imageService: ImageService,
    private invoiceService: InvoiceService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {  
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.bookingId = +id;
        this.bookingService.getOne(this.bookingId).subscribe({
          next: (response) => {
            this.booking = response;
            this.vehicleId = response.vehicleId;
            this.invoiceForm = this.fb.group({
              bookingId: [this.booking.id, Validators.required],
              amount: [null, [Validators.required, Validators.min(0)]],
              serviceDetails: ['', Validators.required]
            });
            this.imageService.getAllByBookingID(this.bookingId).subscribe({
              next: (response) => {
                this.images = response;
              },
              error: (error) => {
                this.errorMessage = error.error.error ?? "Error. Please try again later";
              }
            })
            this.startAutoSlide();
          },
          error: (error) => {
            this.errorMessage = error.error.error ?? "Error. Please try again later";
          }
        });
      }
    });
  }

  ngOnDestroy() {
    clearInterval(this.intervalId);
  }

  startAutoSlide() {
    this.intervalId = setInterval(() => {
      if (this.images && this.images.length > 0) {
        this.currentIndex = (this.currentIndex + 1) % this.images.length;
      }
    }, 3000); // Change every 3 seconds
  }

  next() {
    this.currentIndex = (this.currentIndex + 1) % this.images.length;
  }

  prev() {
    this.currentIndex =
      (this.currentIndex - 1 + this.images.length) % this.images.length;
  }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      this.base64Image = reader.result as string;
    };
    reader.readAsDataURL(file); // This gives you a full Base64 string like "data:image/jpeg;base64,..."
  }

  uploadImage(): void {
    if (this.base64Image == "") return;

    const payload = {
      Base64Data: this.base64Image,
      BookingId: this.bookingId,
      VehicleID: this.vehicleId
    };

    this.imageService.uploadImage(payload).subscribe({
      next: res => console.log('Image uploaded successfully', res),
      error: err => console.error('Upload failed', err)
    });
  }

  deleteImage(index: number): void {
    var img = this.images[index];

    this.imageService.deleteImage(img.id).subscribe({
      next: res => console.log('Image deleted successfully', res),
      error: err => console.error('Delete failed', err)
    })

    this.images.splice(index, 1);
    if (this.currentIndex >= this.images.length) {
      this.currentIndex = this.images.length - 1;
    }
  }

  downloadInvoice() {
    this.router.navigate([`/invoice/${this.booking.id}`]);
  }

  openInvoiceModal(): void {
    if (!this.booking || !this.booking.id || !this.invoiceForm) return;
    this.invoiceForm.patchValue({ bookingId: this.booking.id });
    this.isInvoiceModalOpen = true;
  }

  closeInvoiceModal(): void {
    this.isInvoiceModalOpen = false;
  }

  createInvoice() {
    if (this.invoiceForm.invalid) return;

    const payload = this.invoiceForm.value;
    this.invoiceService.createInvoice(payload).subscribe({
      next: (response) => {
        this.closeInvoiceModal();
        this.router.navigate([`/invoice/${this.booking.id}`]);
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Error creating invoice. Please try again later";
      }
    });
  }

  updateStatus(booking: any): void {
    const newStatus = booking.status;
    this.bookingService.updateStatus(booking.id, newStatus).subscribe({
      next: (response) => {
        booking.status = newStatus;
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Error updating status. Please try again later";
      }
    });
  }

}
