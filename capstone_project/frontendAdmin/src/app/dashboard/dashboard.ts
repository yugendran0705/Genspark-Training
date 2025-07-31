import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../services/profile-service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {


  adminInfo: any = null;
  createdEvents: any[] = [];
  upcomingEvents: any[] = [];
  cancelledEvents: any[] = [];

  totalTicketsSold = 0;
  totalRevenue = 0;

  constructor(private profileservice: ProfileService, private router: Router) { }

  filters = {
    title: '',
    type: '',
    city: ''
  };

  availableCities: string[] = [];

  ngOnInit(): void {
    this.profileservice.getUserProfile().subscribe({
      next: (data: any) => {
        this.adminInfo = data;
        this.createdEvents = data.createdEvents || [];

        this.upcomingEvents = this.createdEvents.filter((e: any) => !e.isCancelled);
        this.cancelledEvents = this.createdEvents.filter((e: any) => e.isCancelled);

        this.availableCities = Array.from(
          new Set(this.upcomingEvents.map((e: any) => e.city))
        );

        this.totalTicketsSold = this.createdEvents.reduce((acc, event) => {
          if (event.isCancelled) return acc;
          return acc + (event.tickets?.reduce((sum: number, t: any) => sum + t.quantity, 0) || 0);
        }, 0);

        this.totalRevenue = this.createdEvents.reduce((acc, event) => {
          if (event.isCancelled) return acc;
          return acc + (event.tickets?.reduce((sum: number, t: any) => sum + t.total, 0) || 0);
        }, 0);
      },
      error: (err) => console.error(err)
    });
  }

  filteredUpcomingEvents(): any[] {
    const now = new Date();

    return this.upcomingEvents.filter((event: any) => {
      const eventDate = new Date(event.date);
      const matchesTitle = !this.filters.title || event.title.toLowerCase().includes(this.filters.title.toLowerCase());
      const matchesCity = !this.filters.city || event.city === this.filters.city;
      const matchesType =
        !this.filters.type ||
        (this.filters.type === 'upcoming' && eventDate > now) ||
        (this.filters.type === 'past' && eventDate < now);

      return matchesTitle && matchesCity && matchesType;
    });
  }

  onAdd() {
    this.router.navigate(['/add'])
  }
  getTicketsSold(event: any): number {
    return event.tickets?.reduce((sum: number, t: any) => sum + t.quantity, 0) || 0;
  }

  getRevenue(event: any): number {
    return event.tickets?.reduce((sum: number, t: any) => sum + t.total, 0) || 0;
  }
}
