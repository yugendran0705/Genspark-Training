import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class PaymentService {
  createOrder(amount: number) {
    return Promise.resolve({ id: 'order_dummy_12345' });
  }
}
