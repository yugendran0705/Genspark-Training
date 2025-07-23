import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { of, throwError } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['login']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [LoginComponent, ReactiveFormsModule, CommonModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    compiled = fixture.nativeElement;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty controls', () => {
    const form = component.loginForm;
    expect(form).toBeTruthy();
    expect(form.get('email')?.value).toBe('');
    expect(form.get('password')?.value).toBe('');
  });

  it('should show validation errors if form is invalid on submit', () => {
    component.loginForm.setValue({ email: '', password: '' });
    component.onSubmit();
    expect(mockAuthService.login).not.toHaveBeenCalled();
  });

  it('should call AuthService and navigate on successful login', fakeAsync(() => {
    const mockResponse = { accessToken: 'abc123', refreshToken: 'xyz789' };
    mockAuthService.login.and.returnValue(of(mockResponse));

    component.loginForm.setValue({ email: 'test@example.com', password: '123456' });
    component.onSubmit();
    tick(); // simulate async

    expect(mockAuthService.login).toHaveBeenCalledWith({
      email: 'test@example.com',
      password: '123456'
    });

    expect(localStorage.getItem('accessToken')).toBe('abc123');
    expect(localStorage.getItem('refreshToken')).toBe('xyz789');
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/profile']);
  }));

  it('should set errorMessage on login failure', fakeAsync(() => {
    mockAuthService.login.and.returnValue(
      throwError(() => ({ error: 'Invalid login' }))
    );

    component.loginForm.setValue({ email: 'fail@example.com', password: 'wrong' });
    component.onSubmit();
    tick();

    expect(component.errorMessage).toBe('Invalid email or password.');
  }));

  it('should redirect to /profile if accessToken already exists on init', () => {
    localStorage.setItem('accessToken', 'token');
    component.ngOnInit();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/profile']);
  });
});
