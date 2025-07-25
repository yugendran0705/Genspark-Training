import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationService {
  eventDetail = signal<any>(null);
  ticketCount = signal<number>(1);
  useWallet = signal<boolean>(false);
  constructor() {}
  
  setBookingData(event: any, count: number): void {
    this.eventDetail.set(event);
    this.ticketCount.set(count);
  }

}
