import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { SlotComponent } from './slot.component';
import { SlotService } from '../services/slot.service';
import { VehicleService } from '../services/vehicle.service';
import { BookingService } from '../services/book.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('SlotComponent', () => {
  let component: SlotComponent;
  let fixture: ComponentFixture<SlotComponent>;
  let mockSlotService: jasmine.SpyObj<SlotService>;
  let mockVehicleService: jasmine.SpyObj<VehicleService>;
  let mockBookingService: jasmine.SpyObj<BookingService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockSlotService = jasmine.createSpyObj('SlotService', ['getAvailable']);
    mockVehicleService = jasmine.createSpyObj('VehicleService', ['getAllByUser']);
    mockBookingService = jasmine.createSpyObj('BookingService', ['create']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [SlotComponent, ReactiveFormsModule, FormsModule, CommonModule],
      providers: [
        { provide: SlotService, useValue: mockSlotService },
        { provide: VehicleService, useValue: mockVehicleService },
        { provide: BookingService, useValue: mockBookingService },
        { provide: Router, useValue: mockRouter },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(SlotComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch slots and sort them by date on init', fakeAsync(() => {
    const mockSlots = [
      { id: 2, slotDateTime: '2025-08-01T10:00:00Z' },
      { id: 1, slotDateTime: '2025-07-01T10:00:00Z' },
    ];
    mockSlotService.getAvailable.and.returnValue(of(mockSlots));
    mockVehicleService.getAllByUser.and.returnValue(of([{ id: 1 }]));

    component.ngOnInit();
    tick();

    expect(component.slots.length).toBe(2);
    expect(component.slots[0].id).toBe(1); // earliest date first
    expect(component.errorMessage).toBe('');
  }));

  it('should show error if fetching slots fails', fakeAsync(() => {
    mockSlotService.getAvailable.and.returnValue(
      throwError(() => ({ error: { error: { error: 'Failed to load slots' } } }))
    );
    mockVehicleService.getAllByUser.and.returnValue(of([]));

    component.ngOnInit();
    tick();

    expect(component.errorMessage).toBe('Failed to load slots');
  }));

  it('should show error if no vehicles found', fakeAsync(() => {
    mockSlotService.getAvailable.and.returnValue(of([]));
    mockVehicleService.getAllByUser.and.returnValue(of([]));

    component.ngOnInit();
    tick();

    expect(component.errorMessage).toBe('No Vehicles Found. Please add the vehicle details in profile page.');
  }));

  it('should show vehicle fetch error', fakeAsync(() => {
    mockSlotService.getAvailable.and.returnValue(of([]));
    mockVehicleService.getAllByUser.and.returnValue(
      throwError(() => ({ error: { error: { error: 'Vehicle error' } } }))
    );

    component.ngOnInit();
    tick();

    expect(component.errorMessage).toBe('Vehicle error');
  }));

  it('should not book slot if no vehicle is selected', () => {
    component.vehicleId = 0;
    component.book(1);
    expect(component.errorMessage).toBe('Select a Vehicle');
    expect(mockBookingService.create).not.toHaveBeenCalled();
  });

  it('should call bookingService and navigate to /bookings if booking is successful', fakeAsync(() => {
    component.vehicleId = 101;
    const slotId = 201;
    mockBookingService.create.and.returnValue(of({ message: 'Booked' }));

    component.book(slotId);
    tick();

    expect(mockBookingService.create).toHaveBeenCalledWith({ slotId, vehicleId: 101 });
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/bookings']);
  }));

  it('should show error if booking fails', fakeAsync(() => {
    component.vehicleId = 10;
    mockBookingService.create.and.returnValue(
      throwError(() => ({ error: { error: { error: 'Booking failed' } } }))
    );

    component.book(5);
    tick();

    expect(component.errorMessage).toBe('Booking failed');
  }));
});
