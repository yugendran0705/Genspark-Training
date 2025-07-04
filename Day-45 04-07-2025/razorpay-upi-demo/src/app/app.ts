import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PaymentComponent } from './payment/payment';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PaymentComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'razorpay-upi-demo';
}
