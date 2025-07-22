import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ConfirmationService } from '../services/confirmation-service';
import { EventService } from '../services/event-service';

@Component({
  selector: 'app-event',
  imports: [CommonModule],
  templateUrl: './event.html',
  styleUrl: './event.css'
})
export class Event implements OnInit {
  constructor(private route: ActivatedRoute, private router: Router, private confirmationService: ConfirmationService, private eventservice: EventService) { }
  id: string = "";
  event: any = {}
  similarEvents: any[] = [];
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = params['id'];
      this.eventservice.geteventbyid(Number(this.id)).subscribe((data) => {
        this.event = data;
        this.fetchSimilarEvents();
        console.log(data);
      });
    });
  }
  count = 1;

  fetchSimilarEvents() {
    this.eventservice.getallevents().subscribe((allEvents: any) => {
      console.log(allEvents)
      this.similarEvents = allEvents.filter(
        (e: any) => e.categoryId === this.event.categoryId && e.id !== this.event.id
      );
    });
  }

  increment() {
    this.count = Math.min(this.count + 1, 10);
  }

  decrement() {
    this.count = Math.max(this.count - 1, 1);
  }

  handleBookNow(id: number) {
    this.router.navigate([`/event/${id}`]);
  }

  bookNow() {
    

    //add qty and detail to confirmation service
    const username = localStorage.getItem('username');
    if (username == null) {
      this.router.navigate(['/login']);
    }
    else {
      this.confirmationService.setBookingData(this.event, this.count);
      this.router.navigate([`/confirmbooking/${this.id}`])
    }
  }

  gotohome() {
    this.router.navigate(['/'])
  }


}
