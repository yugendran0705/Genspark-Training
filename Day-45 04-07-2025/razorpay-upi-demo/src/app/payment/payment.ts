// src/app/payment/payment.component.ts

import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

declare global { interface Window { Razorpay: any } }

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    HttpClientModule
  ],
  templateUrl: './payment.html',
  styleUrls: ['./payment.css']
})
export class PaymentComponent implements OnInit {
  message = '';
  paymentForm: any; // FormGroup
  constructor(
    private fb: FormBuilder,
    private http: HttpClient
  ) {
    this.paymentForm = this.fb.group({
      amount:  [null, [Validators.required, Validators.min(1)]],
      name:    ['', Validators.required],
      email:   ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]]
    });
  }


  get f() { return this.paymentForm.controls; }

  ngOnInit() {
    if (!window.Razorpay) {
      const s = document.createElement('script');
      s.src   = 'https://checkout.razorpay.com/v1/checkout.js';
      s.async = true;
      document.body.appendChild(s);
    }
  }

  pay(): Error | void {
    if (this.paymentForm.invalid) {
      this.message = 'Please fill all required fields correctly.';
      return Error(this.message);
    }

    const amt = this.f.amount.value * 100;
    try {
    this.http
      .post<{ id: string; amount: number; currency: string }>(
        'http://localhost:3000/orders',
        { amount: amt }
      )
      .subscribe({
        next: o => {
          const options = {
            key:      'rzp_test_c3ua0PWpyWlc9U',
            amount:   o.amount,
            currency: o.currency ?? 'INR',
            name:     this.f.name.value,
            order_id: o.id,
            prefill: {
              name:    this.f.name.value,
              email:   this.f.email.value,
              contact: this.f.contact.value,
              vpa:     'success@razorpay'
            },
            handler: () => { this.message = 'Payment successful!'; },
            modal: { ondismiss: () => this.message = 'Payment cancelled' }
          };
          new window.Razorpay(options).open();
        },
        error: _ => {
          this.message = 'Failed to create order. Please try again.';
        }
      });
  }
  catch (e) {
      this.message = 'An unexpected error occurred. Please try again.';
    }
  }
}
