import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Register } from './register';
import { Loginservice } from '../services/loginservice';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('Register', () => {
  let component: Register;
  let fixture: ComponentFixture<Register>;
  let loginServiceSpy: jasmine.SpyObj<Loginservice>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    loginServiceSpy = jasmine.createSpyObj('Loginservice', ['checkUserExists', 'register']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [Register],
      providers: [
        { provide: Loginservice, useValue: loginServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Register);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set emailExists to true if user exists', () => {
    loginServiceSpy.checkUserExists.and.returnValue(of({ email: 'test@mail.com' }));
    component.registerForm.get('email')?.setValue('test@mail.com');
    component.checkEmail();
    expect(component.emailExists).toBeTrue();
  });

  it('should move to next step if user does not exist (404)', () => {
    loginServiceSpy.checkUserExists.and.returnValue(throwError(() => ({ status: 404 })));
    component.registerForm.get('email')?.setValue('new@mail.com');
    component.checkEmail();
    expect(component.emailExists).toBeFalse();
    expect(component.currentStep).toBe(2);
  });

  it('should call register and show toast on successful registration', fakeAsync(() => {
    component.currentStep = 2;
    component.registerForm.setValue({
      name: 'Test',
      email: 'test@mail.com',
      password: 'Test@123',
      confirmPassword: 'Test@123',
      phone: '1234567890',
      role: 'user'
    });
    loginServiceSpy.register.and.returnValue(of({}));
    component.onRegisterClick();
    expect(loginServiceSpy.register).toHaveBeenCalled();
    expect(component.showToast).toBeTrue();
    tick(2000);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/login']);
  }));

  it('should alert on registration error', () => {
    spyOn(window, 'alert');
    component.currentStep = 2;
    component.registerForm.setValue({
      name: 'Test',
      email: 'test@mail.com',
      password: 'Test@123',
      confirmPassword: 'Test@123',
      phone: '1234567890',
      role: 'user'
    });
    loginServiceSpy.register.and.returnValue(throwError(() => ({ error: 'Registration failed' })));
    component.onRegisterClick();
    expect(window.alert).toHaveBeenCalledWith('Registration failed');
  });
});