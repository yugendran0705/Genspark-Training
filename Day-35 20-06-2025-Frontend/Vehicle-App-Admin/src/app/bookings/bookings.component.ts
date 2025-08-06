import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BookingService } from '../services/booking.service';

@Component({
  selector: 'app-bookings',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.css'
})
export class BookingsComponent implements OnInit {
  errorMessage: string = '';
  bookings: any[] = [];
  filteredBookings: any[] = [];
  searchTerm: string = '';

  constructor(
    private bookingService: BookingService,
    private router: Router
  ) {}

  ngOnInit(): void { 
    this.bookingService.getAll().subscribe({
      next: (response) => {
        this.bookings = response;
        this.filteredBookings = response;
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Error. Please try again later";
      }
    });
  }

  goToBooking(id: number): void {
    this.router.navigate([`/booking/${id}`]);
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();
    this.filteredBookings = this.bookings.filter(b =>
      b.name?.toLowerCase().includes(term)
    );
  }
}
