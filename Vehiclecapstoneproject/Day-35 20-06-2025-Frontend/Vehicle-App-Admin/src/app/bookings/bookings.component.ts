import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BookingService } from '../services/booking.service';

@Component({
  selector: 'app-bookings',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.css'
})
export class BookingsComponent implements OnInit{
  errorMessage: string = '';
  bookings: any[] = [];
  
  constructor(
    private bookingService: BookingService,
    private router: Router
  ) {}

  ngOnInit(): void { 
    this.bookingService.getAll().subscribe({
      next: (response) => {
        this.bookings = response;
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Error. Please try again later";
      }
    })
  }

  goToBooking(id: number): void{
    this.router.navigate([`/booking/${id}`]);
  }

}
