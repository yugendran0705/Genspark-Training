import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Login } from './login';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Loginservice } from '../services/loginservice';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('Login', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let loginServiceSpy: jasmine.SpyObj<Loginservice>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    loginServiceSpy = jasmine.createSpyObj('Loginservice', ['login', 'setlogintrue', 'getuserdetails']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [Login, HttpClientTestingModule],
      providers: [
        { provide: Loginservice, useValue: loginServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should login successfully and show toast', fakeAsync(() => {
    const mockResponse = { token: 'abc', username: 'user', role: 'user' };
    loginServiceSpy.login.and.returnValue(of(mockResponse));
    spyOn(localStorage, 'setItem');
    component.loginForm.setValue({ username: 'user', password: 'pass' });
    component.onLoginClick();
    expect(loginServiceSpy.login).toHaveBeenCalledWith({username:'user',password: 'pass'});
    expect(loginServiceSpy.setlogintrue).toHaveBeenCalled();
    expect(loginServiceSpy.getuserdetails).toHaveBeenCalled();
    expect(component.showToast).toBeTrue();
    tick(2000);
    expect(component.showToast).toBeFalse();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/']);
  }));

  it('should alert on login error', () => {
    loginServiceSpy.login.and.returnValue(throwError(() => ({ error: 'Invalid credentials' })));
    spyOn(window, 'alert');
    component.loginForm.setValue({ username: 'user', password: 'wrong' });
    component.onLoginClick();
    expect(window.alert).toHaveBeenCalledWith('Login failed, Invalid credentials');
  });
});