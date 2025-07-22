import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Home } from './home';
import { EventService } from '../services/event-service';
import { NotificationService } from '../services/notification-service';
import { Router } from '@angular/router';
import { of, Subject } from 'rxjs';

describe('Home', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;
  let eventServiceSpy: jasmine.SpyObj<EventService>;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;
  let routerSpy: jasmine.SpyObj<Router>;

  const mockEvents = [
    {
      id: 1,
      title: 'Concert X',
      category: { name: 'Concert' },
      categoryId: 1,
      city: 'CityA',
      date: new Date(Date.now() + 86400000).toISOString(),
      imageurl: '',
      price: 100
    },
    {
      id: 2,
      title: 'Movie Y',
      category: { name: 'Movie' },
      categoryId: 2,
      city: 'CityB',
      date: new Date(Date.now() + 172800000).toISOString(),
      imageurl: '',
      price: 200
    }
  ];

  beforeEach(async () => {
    eventServiceSpy = jasmine.createSpyObj('EventService', ['getallevents']);
    notificationServiceSpy = jasmine.createSpyObj('NotificationService', ['startConnection'], {
      eventRecieved$: new Subject<void>()
    });
    routerSpy = jasmine.createSpyObj('Router', ['navigateByUrl']);

    eventServiceSpy.getallevents.and.returnValue(of(mockEvents));

    await TestBed.configureTestingModule({
      imports: [Home],
      providers: [
        { provide: EventService, useValue: eventServiceSpy },
        { provide: NotificationService, useValue: notificationServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch and categorize events on init', () => {
    expect(eventServiceSpy.getallevents).toHaveBeenCalled();
    expect(component.allEvents.length).toBe(2);
    expect(component.concerts.length).toBe(1);
    expect(component.Movies.length).toBe(1);
  });

  it('should filter events by upcoming date', () => {
    const filtered = component.filteredEvents();
    expect(filtered.length).toBe(2); 
  });

  it('should navigate to event page on handleBookNow', () => {
    component.handleBookNow(1);
    expect(routerSpy.navigateByUrl).toHaveBeenCalledWith('/event/1');
  });
});