import { Component, OnInit } from '@angular/core';
import { InvoiceService } from '../services/invoice.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-invoices',
  imports: [CommonModule],
  templateUrl: './invoices.component.html',
  styleUrl: './invoices.component.css'
})
export class InvoicesComponent implements OnInit {
  invoices: any[] = [];

  constructor(private invoiceService: InvoiceService, private router: Router) {}

  ngOnInit(): void {
    this.invoiceService.getAllInvoices().subscribe({
      next: (data) => this.invoices = data,
      error: () => console.error('Failed to fetch invoices')
    });
  }

  viewInvoice(id: number): void {
    this.router.navigate([`/invoice/${id}`]);
  }
}