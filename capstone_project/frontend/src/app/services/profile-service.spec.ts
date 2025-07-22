import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProfileService } from './profile-service';

describe('ProfileService', () => {
  let service: ProfileService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProfileService]
    });
    service = TestBed.inject(ProfileService);
    httpMock = TestBed.inject(HttpTestingController);
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      if (key === 'username') return 'test@mail.com';
      if (key === 'token') return 'mock-token';
      return null;
    });
  });

  afterEach(() => {
    httpMock.verify();
  });
    it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should set customer name', () => {
    service.setcustomername("TestUser");
    expect(service.customername()).toBe("TestUser");
  });

  it('should fetch user profile with auth header', () => {
    const mockProfile = { name: 'Test', email: 'test@mail.com' };
    service.getUserProfile().subscribe(data => {
      expect(data).toEqual(mockProfile);
    });
    const req = httpMock.expectOne('http://localhost:5136/api/v1/customer/test@mail.com');
    expect(req.request.method).toBe('GET');
    req.flush(mockProfile);
  });

  it('should handle error when fetching user profile', () => {
    service.getUserProfile().subscribe({
      next: () => fail('should have failed'),
      error: (err) => {
        expect(err.status).toBe(404);
      }
    });
    const req = httpMock.expectOne('http://localhost:5136/api/v1/customer/test@mail.com');
    req.flush('Not found', { status: 404, statusText: 'Not Found' });
  });
});
