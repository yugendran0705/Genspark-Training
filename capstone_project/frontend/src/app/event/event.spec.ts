import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Event } from './event';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService } from '../services/confirmation-service';
import { EventService } from '../services/event-service';
import { of } from 'rxjs';

describe('Event', () => {
  let component: Event;
  let fixture: ComponentFixture<Event>;
  let eventServiceSpy: jasmine.SpyObj<EventService>;
  let confirmationServiceSpy: jasmine.SpyObj<ConfirmationService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let activatedRouteStub: any;

  const mockEvent = {
    id: 1,
    title: 'Test Event',
    categoryId: 2,
    category: { name: 'Music' },
    imageurl: 'img.jpg',
    price: 100,
    ticketcount: 10,
    date: new Date().toISOString(),
    address: 'Venue',
    city: 'City',
    description: 'desc',
    context: 'context'
  };

  const mockSimilarEvents = [
    { id: 2, categoryId: 2, title: 'Similar 1', imageurl: '', price: 50, category: { name: 'Music' } },
    { id: 3, categoryId: 2, title: 'Similar 2', imageurl: '', price: 60, category: { name: 'Music' } }
  ];

  beforeEach(async () => {
    eventServiceSpy = jasmine.createSpyObj('EventService', ['geteventbyid', 'getallevents']);
    confirmationServiceSpy = jasmine.createSpyObj('ConfirmationService', ['setBookingData']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    activatedRouteStub = {
      params: of({ id: '1' })
    };

    eventServiceSpy.geteventbyid.and.returnValue(of(mockEvent));
    eventServiceSpy.getallevents.and.returnValue(of([mockEvent, ...mockSimilarEvents]));

    await TestBed.configureTestingModule({
      imports: [Event],
      providers: [
        { provide: EventService, useValue: eventServiceSpy },
        { provide: ConfirmationService, useValue: confirmationServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: activatedRouteStub }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Event);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch event by id on init', () => {
    expect(eventServiceSpy.geteventbyid).toHaveBeenCalledWith(1);
    expect(component.event.title).toBe('Test Event');
  });

  it('should fetch similar events', () => {
    component.event = mockEvent;
    component.fetchSimilarEvents();
    expect(eventServiceSpy.getallevents).toHaveBeenCalled();
    expect(component.similarEvents.length).toBe(2);
    expect(component.similarEvents[0].id).not.toBe(component.event.id);
  });

  it('should increment and decrement ticket count within bounds', () => {
    component.count = 1;
    component.increment();
    expect(component.count).toBe(2);
    component.count = 10;
    component.increment();
    expect(component.count).toBe(10);
    component.count = 2;
    component.decrement();
    expect(component.count).toBe(1);
    component.decrement();
    expect(component.count).toBe(1);
  });

  it('should navigate to login if not logged in when booking', () => {
    spyOn(localStorage, 'getItem').and.returnValue(null);
    component.bookNow();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should set booking data and navigate to confirmbooking if logged in', () => {
    spyOn(localStorage, 'getItem').and.returnValue('user@mail.com');
    component.event = mockEvent;
    component.count = 3;
    component.bookNow();
    expect(confirmationServiceSpy.setBookingData).toHaveBeenCalledWith(mockEvent, 3);
    expect(routerSpy.navigate).toHaveBeenCalledWith([`/confirmbooking/${component.id}`]);
  });

  it('should navigate to home on gotohome()', () => {
    component.gotohome();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/']);
  });
});