import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationService {
  eventDetail = signal<any>(null);
  ticketCount = signal<number>(1);

  constructor() {}

  setBookingData(event: any, count: number) {
    this.eventDetail.set(event);
    this.ticketCount.set(count);
  }
}
