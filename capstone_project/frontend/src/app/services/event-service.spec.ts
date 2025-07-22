import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EventService } from './event-service';

describe('EventService', () => {
  let service: EventService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EventService]
    });
    service = TestBed.inject(EventService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch event by id', () => {
    const mockEvent = { id: 1, title: 'Test Event' };
    service.geteventbyid(1).subscribe(event => {
      expect(event).toEqual(mockEvent);
    });
    const req = httpMock.expectOne('http://localhost:5136/api/events/1');
    expect(req.request.method).toBe('GET');
    req.flush(mockEvent);
  });

  it('should fetch all events', () => {
    const mockEvents = [{ id: 1, title: 'Event 1' }, { id: 2, title: 'Event 2' }];
    service.getallevents().subscribe(events => {
      expect(events).toEqual(mockEvents);
    });
    const req = httpMock.expectOne('http://localhost:5136/api/events');
    expect(req.request.method).toBe('GET');
    req.flush(mockEvents);
  });

  it('should handle error for geteventbyid', () => {
    service.geteventbyid(99).subscribe({
      next: () => fail('should have failed with 404 error'),
      error: (error) => {
        expect(error.status).toBe(404);
      }
    });
    const req = httpMock.expectOne('http://localhost:5136/api/events/99');
    req.flush('Event not found', { status: 404, statusText: 'Not Found' });
  });
});