import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VehicleService } from '../services/vehicle.service';
import { SlotService } from '../services/slot.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BookingService } from '../services/book.service';

@Component({
  selector: 'app-slot',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './slot.component.html',
  styleUrl: './slot.component.css'
})
export class SlotComponent implements OnInit {
  errorMessage: string = '';
  slots: any[] = [];
  vehicles: any[] = [];
  vehicleId: number = 0;
  pendingBookings: number = 0;

  constructor(
    private slotService: SlotService,
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private bookingService: BookingService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.slotService.getAvailable().subscribe({
      next: (response) => {
        this.slots = response;
      },
      error: (error) => {
        this.errorMessage = error.error.error.error ?? "Error in Getting Service Slots";
      }
    })



    this.slots = this.slots.sort((a, b) => new Date(a.slotDateTime).getTime() - new Date(b.slotDateTime).getTime());
    this.vehicleService.getAllByUser().subscribe({
      next: (response) => {
        if (response.length == 0) {
          if (!this.errorMessage) {
            this.errorMessage = "No Vehicles Found. Please add the vehicle details in profile page.";
          }
        }
        this.vehicles = response;
      },
      error: (error) => {
        this.errorMessage = error.error.error.error ?? "Error in Getting User Vehicles";
      }
    })
  }

  // selectSlot(id: number): void {
  //   this.slotId = id;
  // }

  book(slotId: number): void {
    if (this.vehicleId == 0) {
      this.errorMessage = "Select a Vehicle"
    }
    else {
      console.log(slotId, this.vehicleId)


      //check for the existing pending bookings
      this.bookingService.getAllByUser().subscribe({
        next: (response) => {
          this.pendingBookings = response.filter((booking: any) => booking.status === 'pending').length;
          console.log(this.pendingBookings);
          if (this.pendingBookings > 2) {
            this.errorMessage = `You already have ${this.pendingBookings} pending bookings.`;
          }
          else {
            this.bookingService.create({ slotId: slotId, vehicleId: this.vehicleId }).subscribe({
              next: (response) => {
                this.router.navigate(['/bookings'])
              },
              error: (error) => {
                this.errorMessage = error.error.error.error ?? "Error in Booking Slot";
              }
            })

          }
        },
        error: (error) => {
          this.errorMessage = error.error.error.error ?? "Error in Getting User Bookings";
        }
      })



    }
  }

}
