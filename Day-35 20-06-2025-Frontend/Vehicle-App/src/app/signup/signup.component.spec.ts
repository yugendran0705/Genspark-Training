import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SignupComponent } from './signup.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { of, throwError } from 'rxjs';

describe('SignupComponent', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;
  let mockUserService: jasmine.SpyObj<UserService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockUserService = jasmine.createSpyObj('UserService', ['create']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [CommonModule, ReactiveFormsModule, SignupComponent],
      providers: [
        { provide: UserService, useValue: mockUserService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the SignupComponent', () => {
    expect(component).toBeTruthy();
  });

  it('should invalidate form if password and confirmPassword do not match', () => {
    component.signupForm.setValue({
      name: 'Test User',
      email: 'test@example.com',
      phone: '9876543210',
      password: 'password123',
      confirmPassword: 'different'
    });

    expect(component.signupForm.valid).toBeFalse();
    expect(component.signupForm.errors?.['mismatch']).toBeTrue();
  });

  it('should mark form as touched if invalid on submit', () => {
    spyOn(component.signupForm, 'markAllAsTouched');

    component.signupForm.setValue({
      name: '',
      email: '',
      phone: '',
      password: '',
      confirmPassword: ''
    });

    component.onSubmit();
    expect(component.signupForm.markAllAsTouched).toHaveBeenCalled();
  });

  it('should call userService.create with correct payload on valid submit', () => {
    component.signupForm.setValue({
      name: 'Yugendran',
      email: 'yugen@example.com',
      phone: '9876543210',
      password: 'secure123',
      confirmPassword: 'secure123'
    });

    mockUserService.create.and.returnValue(of({}));

    component.onSubmit();

    expect(mockUserService.create).toHaveBeenCalledWith({
      name: 'Yugendran',
      email: 'yugen@example.com',
      phone: '9876543210',
      password: 'secure123'
    });

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should set errorMessage on signup failure', () => {
    component.signupForm.setValue({
      name: 'Fail User',
      email: 'fail@example.com',
      phone: '9876543210',
      password: 'pass123',
      confirmPassword: 'pass123'
    });

    mockUserService.create.and.returnValue(throwError(() => ({
      error: 'Email already exists'
    })));

    component.onSubmit();
    expect(component.errorMessage).toBe('Email already exists');
  });

  it('should fallback to default error message if none provided', () => {
    component.signupForm.setValue({
      name: 'Fail User',
      email: 'fail@example.com',
      phone: '9876543210',
      password: 'pass123',
      confirmPassword: 'pass123'
    });

    mockUserService.create.and.returnValue(throwError(() => ({
      error: null
    })));

    component.onSubmit();
    expect(component.errorMessage).toBe('Signup failed. Please try again.');
  });
});
