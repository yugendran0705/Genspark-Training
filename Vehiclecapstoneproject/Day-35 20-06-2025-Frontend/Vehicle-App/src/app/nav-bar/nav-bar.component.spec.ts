import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NavBarComponent } from './nav-bar.component';
import { RouterTestingModule } from '@angular/router/testing';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { By } from '@angular/platform-browser';

describe('NavBarComponent', () => {
  let component: NavBarComponent;
  let fixture: ComponentFixture<NavBarComponent>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        CommonModule,
        ReactiveFormsModule,
        NavBarComponent,  // Import NavBarComponent directly here
      ],
      providers: [
        {
          provide: AuthService,
          useValue: {} // Stubbed AuthService
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NavBarComponent);
    component = fixture.componentInstance;
    compiled = fixture.nativeElement as HTMLElement;
  });

  afterEach(() => {
    localStorage.clear(); // Clear tokens after each test
  });

  it('should create the NavBarComponent', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should render "AutoCare" in the top logo link', () => {
    fixture.detectChanges();
    const logo = compiled.querySelector('a.text-2xl');
    expect(logo?.textContent?.trim()).toContain('AutoCare');
  });

  it('should show Login and Signup links when not logged in', () => {
    localStorage.removeItem('accessToken');
    fixture.detectChanges();

    const loginLink = compiled.querySelector('a[routerLink="/login"]');
    const signupLink = compiled.querySelector('a[routerLink="/signup"]');
    const profileLink = compiled.querySelector('a[routerLink="/profile"]');

    expect(loginLink).toBeTruthy();
    expect(signupLink).toBeTruthy();
    expect(profileLink).toBeFalsy(); // Profile link should not be shown
  });

  it('should show Profile link when logged in', () => {
    localStorage.setItem('accessToken', 'mock-token');
    fixture.detectChanges();

    // Force change detection to call ngDoCheck
    component.ngDoCheck();
    fixture.detectChanges();

    const profileLink = compiled.querySelector('a[routerLink="/profile"]');
    const loginLink = compiled.querySelector('a[routerLink="/login"]');
    const signupLink = compiled.querySelector('a[routerLink="/signup"]');

    expect(profileLink).toBeTruthy();
    expect(loginLink).toBeFalsy(); // Login and Signup should not show
    expect(signupLink).toBeFalsy();
  });

  it('should toggle mobile menu when button is clicked', () => {
    fixture.detectChanges();
    const button = compiled.querySelector('button[aria-label="Toggle menu"]') as HTMLElement;
    expect(component.mobileMenuOpen).toBeFalse();

    button.click();
    fixture.detectChanges();

    expect(component.mobileMenuOpen).toBeTrue();

    const mobileMenu = compiled.querySelector('div.md\\:hidden');
    expect(mobileMenu).toBeTruthy();
  });

  it('should show correct auth links in mobile menu when not logged in', () => {
    localStorage.removeItem('accessToken');
    fixture.detectChanges();
    component.mobileMenuOpen = true;
    fixture.detectChanges();

    const login = compiled.querySelector('div.md\\:hidden a[routerLink="/login"]');
    const signup = compiled.querySelector('div.md\\:hidden a[routerLink="/signup"]');
    const profile = compiled.querySelector('div.md\\:hidden a[routerLink="/profile"]');

    expect(login).toBeTruthy();
    expect(signup).toBeTruthy();
    expect(profile).toBeFalsy();
  });

  it('should show profile link in mobile menu when logged in', () => {
    localStorage.setItem('accessToken', 'mock-token');
    fixture.detectChanges();
    component.ngDoCheck();
    component.mobileMenuOpen = true;
    fixture.detectChanges();

    const profile = compiled.querySelector('div.md\\:hidden a[routerLink="/profile"]');
    const login = compiled.querySelector('div.md\\:hidden a[routerLink="/login"]');
    const signup = compiled.querySelector('div.md\\:hidden a[routerLink="/signup"]');

    expect(profile).toBeTruthy();
    expect(login).toBeFalsy();
    expect(signup).toBeFalsy();
  });
});
