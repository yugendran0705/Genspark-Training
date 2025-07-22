import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIf, NgFor } from '@angular/common';
//@ts-ignore
import * as bootstrap from 'bootstrap';
import { FormsModule } from '@angular/forms';
import { TicketService } from '../services/ticket-service';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule, NgIf, NgFor,FormsModule],
  templateUrl: './history.html',
  styleUrl: './history.css'
})
export class History implements OnInit {
  tickets: any[] = [];
  selectedTicketId: number | null = null;

  filters = {
    search: '',
    minPrice: null,
    maxPrice: null,
    category: '',
    city: '',
    status: 'all'
  };

  categories: string[] = [];
  cities: string[] = [];

  constructor(private ticketservice:TicketService) {}

  ngOnInit(): void {
    const email = localStorage.getItem('username');
    if (email) {
      this.ticketservice.gettickets(email).subscribe({
        next: (data) => {
          console.log(data)
          const now = new Date();
          this.tickets = data.map((ticket:any) => {
            const eventDate = new Date(ticket.event?.date);
            ticket.eventDate = eventDate;
            ticket.isUpcoming = eventDate > now;
            ticket.timeToEvent = this.getRelativeTime(eventDate, now);
            return ticket;
          }).sort((a:any, b:any) => a.eventDate.getTime() - b.eventDate.getTime());

          // Extracting unique categories and cities
          this.categories = [...new Set(this.tickets.map(t => t.event?.category?.name).filter(Boolean))];
          this.cities = [...new Set(this.tickets.map(t => t.event?.city).filter(Boolean))];
        },
        error: (err) => {
          console.error('Failed to fetch tickets:', err);
        }
      });
    }
  }

  getRelativeTime(eventDate: Date, currentDate: Date): string {
    const diff = eventDate.getTime() - currentDate.getTime();
    const diffInSeconds = Math.floor(diff / 1000);
    if (diffInSeconds < 0) {
      const pastSecs = Math.abs(diffInSeconds);
      const hours = Math.floor(pastSecs / 3600);
      const days = Math.floor(hours / 24);
      if (days >= 1) return `${days} day(s) ago`;
      if (hours >= 1) return `${hours} hour(s) ago`;
      return `a few minutes ago`;
    } else {
      const hours = Math.floor(diffInSeconds / 3600);
      const days = Math.floor(hours / 24);
      if (days >= 1) return `in ${days} day(s)`;
      if (hours >= 1) return `in ${hours} hour(s)`;
      return `soon`;
    }
  }

  filteredTickets() {
    return this.tickets.filter(ticket => {
      const event = ticket.event;
      const titleMatch = event.title.toLowerCase().includes(this.filters.search.toLowerCase());
      const minPriceMatch = this.filters.minPrice == null || ticket.total >= this.filters.minPrice;
      const maxPriceMatch = this.filters.maxPrice == null || ticket.total <= this.filters.maxPrice;
      const statusMatch = this.filters.status === 'all'
        || (this.filters.status === 'upcoming' && ticket.isUpcoming && !ticket.isCancelled)
        || (this.filters.status === 'completed' && !ticket.isUpcoming && !ticket.isCancelled)
        || (this.filters.status === 'cancelled' && ticket.isCancelled);
      return titleMatch && minPriceMatch && maxPriceMatch  && statusMatch;
    });
  }

  openCancelModal(ticketId: number) {
    this.selectedTicketId = ticketId;
    const modalEl = document.getElementById('cancelModal');
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();
  }

  confirmCancel() {
    if (this.selectedTicketId !== null) {
      this.ticketservice.cancelTicket(this.selectedTicketId).subscribe({
        next: () => {
          const ticket = this.tickets.find(t => t.id === this.selectedTicketId);
          if (ticket) ticket.isCancelled = true;

          this.selectedTicketId = null;
          const modalEl = document.getElementById('cancelModal');
          const modal = bootstrap.Modal.getInstance(modalEl);
          modal?.hide();
        },
        error: (err) => {
          console.error('Cancel failed:', err);
          alert('Failed to cancel ticket. Please try again.');
        }
      });
    }
  }
}
