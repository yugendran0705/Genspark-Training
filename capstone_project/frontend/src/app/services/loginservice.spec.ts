import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Loginservice } from './loginservice';

describe('Loginservice', () => {
  let service: Loginservice;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [Loginservice]
    });
    service = TestBed.inject(Loginservice);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call login API and return data', () => {
    const mockResponse = { token: 'abc', username: 'user', role: 'user' };
    service.login({username:'user', password:'pass'}).subscribe(data => {
      expect(data).toEqual(mockResponse);
    });
    const req = httpMock.expectOne('http://localhost:5136/api/v1/authentication');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ username: 'user', password: 'pass' });
    req.flush(mockResponse);
  });

  it('should call checkUserExists and return user data', () => {
    const mockUser = { email: 'test@mail.com', name: 'Test' };
    service.checkUserExists('test@mail.com').subscribe(data => {
      expect(data).toEqual(mockUser);
    });
    const req = httpMock.expectOne('http://localhost:5136/api/v1/customer/test@mail.com');
    expect(req.request.method).toBe('GET');
    req.flush(mockUser);
  });

  it('should call register for user', () => {
    const mockObj = { 
      email: 'test@mail.com', 
      password: 'pass', 
      name: 'Test User', 
      phoneNumber: '1234567890' 
    };
    const mockResponse = { success: true };
    service.register(mockObj, 'user').subscribe(data => {
      expect(data).toEqual(mockResponse);
    });
    const req = httpMock.expectOne('http://localhost:5136/api/v1/customer/register');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockObj);
    req.flush(mockResponse);
  });
});
