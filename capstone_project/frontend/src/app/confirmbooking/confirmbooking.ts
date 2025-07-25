import { Component, OnInit, computed } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService } from '../services/confirmation-service';
import { CommonModule } from '@angular/common';
import { TicketService } from '../services/ticket-service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-confirmbooking',
  imports: [CommonModule, FormsModule],
  templateUrl: './confirmbooking.html',
  styleUrl: './confirmbooking.css'
})
export class Confirmbooking implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private confirmationService: ConfirmationService,
    private ticketservice:TicketService,
  ) {}
  

  event: any;
  count: any;
  showToast: boolean = false;
  useWallet!: boolean;

  ngOnInit(): void {
    this.event = this.confirmationService.eventDetail;
    this.count = this.confirmationService.ticketCount;
    this.useWallet = this.confirmationService.useWallet();
    console.log("Confirmed Event:", this.event());
    console.log("Ticket Count:", this.count());
  }

  confirmBooking() {
     this.ticketservice.bookticket({
      eventName:this.event().title, 
      quantity:this.count(),
      useWallet:this.useWallet,
    }).subscribe({
      next: (response: Blob) => {
        const blob = new Blob([response], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Ticket_${this.event.title}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(url); // Clean up
        this.showToast = true;
        
        setTimeout(() => {
          this.showToast = false;
          this.router.navigate(['/']);
        }, 3000);
      },
      error: (err) => {
        console.error("Error downloading ticket:", err);
        alert("Ticket booking failed. Please try again.");
      }
    });
  }
  closeToast() {
    this.showToast = false;
  }

  cancel(){
    this.router.navigate([`/event/${this.event().id}`])
  }
}

