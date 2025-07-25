import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BookingsComponent } from './bookings.component';
import { of, throwError } from 'rxjs';
import { BookingService } from '../services/book.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

describe('BookingsComponent', () => {
  let component: BookingsComponent;
  let fixture: ComponentFixture<BookingsComponent>;
  let mockBookingService: jasmine.SpyObj<BookingService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockBookingService = jasmine.createSpyObj('BookingService', ['getAllByUser']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [CommonModule, ReactiveFormsModule, BookingsComponent],
      providers: [
        { provide: BookingService, useValue: mockBookingService },
        { provide: Router, useValue: mockRouter },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(BookingsComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load bookings on init', () => {
    const mockBookings = [
      { id: 1, slot: '2025-07-12T10:00:00Z' },
      { id: 2, slot: '2025-07-15T15:00:00Z' }
    ];
    mockBookingService.getAllByUser.and.returnValue(of(mockBookings));

    component.ngOnInit();

    expect(component.bookings).toEqual(mockBookings);
    expect(component.errorMessage).toBe('');
  });

  it('should handle error when booking fetch fails', () => {
    mockBookingService.getAllByUser.and.returnValue(
      throwError(() => ({ error: { error: 'Failed to fetch bookings' } }))
    );

    component.ngOnInit();

    expect(component.bookings.length).toBe(0);
    expect(component.errorMessage).toBe('Failed to fetch bookings');
  });

  it('should navigate to booking details page', () => {
    const bookingId = 42;
    component.goToBooking(bookingId);
    expect(mockRouter.navigate).toHaveBeenCalledWith([`/booking/${bookingId}`]);
  });
});
