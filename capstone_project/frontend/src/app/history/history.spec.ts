import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { History } from './history';
import { TicketService } from '../services/ticket-service';
import { of, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';

describe('History', () => {
  let component: History;
  let fixture: ComponentFixture<History>;
  let ticketServiceSpy: jasmine.SpyObj<TicketService>;

  const mockTickets = [
    {
      id: 1,
      quantity: 2,
      total: 400,
      isCancelled: false,
      bookingDate: new Date(),
      event: {
        title: 'Concert',
        description: 'Music event',
        address: 'Venue',
        city: 'CityA',
        date: new Date(Date.now() + 86400000), // 1 day in future
        category: { name: 'Music' }
      }
    },
    {
      id: 2,
      quantity: 1,
      total: 200,
      isCancelled: true,
      bookingDate: new Date(),
      event: {
        title: 'Past Event',
        description: 'Old event',
        address: 'Venue2',
        city: 'CityB',
        date: new Date(Date.now() - 86400000), // 1 day in past
        category: { name: 'Drama' }
      }
    }
  ];

  beforeEach(async () => {
    ticketServiceSpy = jasmine.createSpyObj('TicketService', ['gettickets', 'cancelTicket']);
    await TestBed.configureTestingModule({
      imports: [History],
      providers: [
        { provide: TicketService, useValue: ticketServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(History);
    component = fixture.componentInstance;
    // Mock localStorage
    spyOn(localStorage, 'getItem').and.callFake((key: string) => key === 'username' ? 'test@mail.com' : null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch tickets on init', () => {
    ticketServiceSpy.gettickets.and.returnValue(of(mockTickets));
    component.ngOnInit();
    expect(ticketServiceSpy.gettickets).toHaveBeenCalledWith('test@mail.com');
    expect(component.tickets.length).toBe(2);
    expect(component.categories.length).toBeGreaterThan(0);
    expect(component.cities.length).toBeGreaterThan(0);
  });

  it('should handle error when fetching tickets', () => {
    ticketServiceSpy.gettickets.and.returnValue(throwError(() => new Error('Failed')));
    spyOn(console, 'error');
    component.ngOnInit();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch tickets:', jasmine.any(Error));
  });

  it('should filter tickets by search', () => {
    component.tickets = mockTickets;
    component.filters.search = 'Concert';
    const filtered = component.filteredTickets();
    expect(filtered.length).toBe(1);
    expect(filtered[0].event.title).toBe('Concert');
  });


  it('should filter tickets by status', () => {
    component.tickets = mockTickets;
    component.filters.status = 'cancelled';
    const filtered = component.filteredTickets();
    expect(filtered.length).toBe(1);
    expect(filtered[0].isCancelled).toBeTrue();
  });

  it('should handle cancel error', fakeAsync(() => {
    component.tickets = mockTickets;
    component.selectedTicketId = mockTickets[0].id;
    ticketServiceSpy.cancelTicket.and.returnValue(throwError(() => new Error('Cancel failed')));
    spyOn(window, 'alert');
    spyOn(document, 'getElementById').and.returnValue({} as any);
    (window as any).bootstrap = { Modal: { getInstance: () => ({ hide: () => {} }) } };
    component.confirmCancel();
    tick();
    expect(window.alert).toHaveBeenCalledWith('Failed to cancel ticket. Please try again.');
  }));
});