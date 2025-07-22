import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TicketService } from './ticket-service';

describe('TicketService', () => {
  let service: TicketService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TicketService]
    });
    service = TestBed.inject(TicketService);
    httpMock = TestBed.inject(HttpTestingController);
    spyOn(localStorage, 'getItem').and.returnValue('mock-token');
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('should fetch tickets with auth header', () => {
    const mockTickets = [{ id: 1, event: { title: 'Event' } }];
    service.gettickets('test@mail.com').subscribe(data => {
      expect(data).toEqual(mockTickets);
    });
    const req = httpMock.expectOne('http://localhost:5136/api/v1/ticket/gettickets/test@mail.com');
    expect(req.request.method).toBe('GET');
    req.flush(mockTickets);
  });

  it('should cancel ticket with auth header', () => {
    service.cancelTicket(123).subscribe();
    const req = httpMock.expectOne('http://localhost:5136/api/v1/ticket/123/cancel');
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });

  it('should book ticket and return blob', () => {
    const mockBlob = new Blob(['test'], { type: 'application/pdf' });
    service.bookticket({ eventName: 'EventName', quantity: 2 }).subscribe(data => {
    expect(data).toBeTruthy();
  });
    const req = httpMock.expectOne('http://localhost:5136/api/v1/ticket/book');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ eventName: 'EventName', quantity: 2 });
    expect(req.request.responseType).toBe('blob');
    req.flush(mockBlob);
  });
});