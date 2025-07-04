// src/app/payment/payment.component.spec.ts

import { TestBed, ComponentFixture, fakeAsync, tick } from '@angular/core/testing';
import { ReactiveFormsModule }      from '@angular/forms';
import { CommonModule }             from '@angular/common';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { PaymentComponent } from './payment';

declare global {
  interface Window { Razorpay: any; }
}

describe('PaymentComponent (standalone)', () => {
  let fixture: ComponentFixture<PaymentComponent>;
  let component: PaymentComponent;
  let httpMock: HttpTestingController;
  let razorpayInstance: any;

  beforeEach(async () => {
    (window as any).Razorpay = () => {};

    await TestBed.configureTestingModule({
      imports: [
        PaymentComponent,
        ReactiveFormsModule,
        CommonModule,
        HttpClientTestingModule
      ]
    }).compileComponents();

    fixture  = TestBed.createComponent(PaymentComponent);
    component = fixture.componentInstance;
    httpMock  = TestBed.inject(HttpTestingController);

    // Replace window.Razorpay with a spy that returns an object with open()
    razorpayInstance = { open: jasmine.createSpy('open') };
    spyOn(window as any, 'Razorpay').and.returnValue(razorpayInstance);

    fixture.detectChanges();
  });

  afterEach(() => {
    // Only verify if httpMock was set
    if (httpMock) {
      httpMock.verify();
    }
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should not call backend or open Razorpay when form is invalid', () => {
    // form is initially invalid
    component.pay();
    expect(component.message)
      .toBe('Please fill all required fields correctly.');
    httpMock.expectNone('http://localhost:3000/orders');
    expect((window as any).Razorpay).not.toHaveBeenCalled();
  });

  it('should create order and open Razorpay on pay() when form is valid', fakeAsync(() => {
    // Arrange valid form
    component.paymentForm.setValue({
      amount:  100,
      name:    'Alice',
      email:   'alice@example.com',
      contact: '9876543210'
    });

    // Act
    component.pay();

    // Expect HTTP POST to your backend
    const req = httpMock.expectOne('http://localhost:3000/orders');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ amount: 100 * 100 });

    // Provide fake response
    const mockOrder = { id: 'order_ABC123', amount: 10000, currency: 'INR' };
    req.flush(mockOrder);
    tick();

    // Verify Razorpay constructor was called with correct options
    expect((window as any).Razorpay).toHaveBeenCalledWith(
      jasmine.objectContaining({
        key:      'rzp_test_c3ua0PWpyWlc9U',
        amount:   mockOrder.amount,
        currency: mockOrder.currency,
        order_id: mockOrder.id,
        name:     'Alice'
      })
    );

    // And that open() was invoked
    expect(razorpayInstance.open).toHaveBeenCalled();
  }));

  it('should set error message when order creation fails', fakeAsync(() => {
    component.paymentForm.setValue({
      amount:  50,
      name:    'Bob',
      email:   'bob@example.com',
      contact: '9123456780'
    });

    component.pay();

    // Simulate network error
    const req = httpMock.expectOne('http://localhost:3000/orders');
    req.error(new ErrorEvent('Network failure'));
    tick();

    expect(component.message)
      .toBe('Failed to create order. Please try again.');
    expect((window as any).Razorpay).not.toHaveBeenCalled();
  }));

  it('should append Razorpay script on ngOnInit if not present', () => {
    // Remove existing global
    delete (window as any).Razorpay;

    // Spy on appendChild
    const appendSpy = spyOn(document.body, 'appendChild');

    component.ngOnInit();

    expect(appendSpy).toHaveBeenCalledWith(jasmine.any(HTMLScriptElement));
    const scriptEl = appendSpy.calls.mostRecent().args[0] as HTMLScriptElement;

    expect(scriptEl.src)
      .toMatch(/https:\/\/checkout\.razorpay\.com\/v1\/checkout\.js$/);
    expect(scriptEl.async).toBeTrue();
  });
});
