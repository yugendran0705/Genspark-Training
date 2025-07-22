import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Navbar } from './navbar';
import { Loginservice } from '../services/loginservice';
import { NotificationService } from '../services/notification-service';
import { of, BehaviorSubject } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

describe('Navbar', () => {
  let component: Navbar;
  let fixture: ComponentFixture<Navbar>;
  let loginServiceSpy: jasmine.SpyObj<Loginservice>;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;

  beforeEach(async () => {
    loginServiceSpy = jasmine.createSpyObj('Loginservice', [
      'logout'
    ], {
      islogged$: new BehaviorSubject<boolean>(true),
      name$: new BehaviorSubject<string>('Test User')
    });

    notificationServiceSpy = jasmine.createSpyObj('NotificationService', [
      'removeNotification'
    ], {
      updates$: of([
        { id: '1', a: 'Title', b: 'Body', c: '', d: '', e: '' }
      ])
    });

    await TestBed.configureTestingModule({
      imports: [Navbar,RouterTestingModule],
      providers: [
        { provide: Loginservice, useValue: loginServiceSpy },
        { provide: NotificationService, useValue: notificationServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Navbar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should subscribe to login status and name', () => {
    expect(component.islogged).toBeTrue();
    expect(component.name).toBe('Test User');
  });

  it('should subscribe to notifications', () => {
    expect(component.notifications.length).toBe(1);
    expect(component.notifications[0].id).toBe('1');
  });

  it('should show and hide toast on logout', fakeAsync(() => {
    component.handlelogout();
    expect(loginServiceSpy.logout).toHaveBeenCalled();
    expect(component.showToast).toBeTrue();
    tick(2000);
    expect(component.showToast).toBeFalse();
  }));

  it('should close toast', () => {
    component.showToast = true;
    component.closeToast();
    expect(component.showToast).toBeFalse();
  });

  it('should toggle notifications', () => {
    component.showNotifications = false;
    component.toggleNotifications();
    expect(component.showNotifications).toBeTrue();
    component.toggleNotifications();
    expect(component.showNotifications).toBeFalse();
  });

  it('should close notifications', () => {
    component.showNotifications = true;
    component.closeNotifications();
    expect(component.showNotifications).toBeFalse();
  });

  it('should remove notification', () => {
    component.removenotification('1');
    expect(notificationServiceSpy.removeNotification).toHaveBeenCalledWith('1');
  });

  it('should close notifications on outside click', () => {
    component.showNotifications = true;
    // Simulate click outside
    const fakeEvent = {
      target: document.createElement('div')
    } as unknown as Event;
    component.onClickOutside(fakeEvent);
    expect(component.showNotifications).toBeFalse();
  });

  it('should not close notifications if click is inside', () => {
    component.showNotifications = true;
    // Simulate click inside
    const insideElem = document.createElement('div');
    insideElem.classList.add('nav-item', 'position-relative');
    const fakeEvent = {
      target: insideElem
    } as unknown as Event;
    component.onClickOutside(fakeEvent);
    expect(component.showNotifications).toBeTrue();
  });
});