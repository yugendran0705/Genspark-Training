import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InvoiceService } from '../services/invoice.service';

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  imports: [CommonModule, ReactiveFormsModule]
})
export class InvoiceComponent implements OnInit {
  invoice: any = null;
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute, 
    private http: HttpClient,
    private invoiceService: InvoiceService
  ) {

  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.invoiceService.getInvoiceByBookingID(id).subscribe({
          next: (data) => {
            this.invoice = data;
            this.loading = false;
          },
          error: () => {
            this.error = 'Failed to fetch invoice.';
            this.loading = false;
          }
        });
    }
  }

  downloadPDF(): void {
    const id = this.invoice?.bookingId;
    if (!id) return;

    this.invoiceService.getInvoicePDFByBookingID(id).subscribe({
      next: (blob: Blob) => {
        const fileName = `invoice_${this.invoice?.id ?? id}.pdf`;
        const url = window.URL.createObjectURL(blob);

        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        a.style.display = 'none';

        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);

        setTimeout(() => {
          window.URL.revokeObjectURL(url);
        }, 500); // slight delay to ensure cleanup
      },
      error: () => {
        alert('Failed to download invoice. Please try again.');
      }
    });
  }


}
