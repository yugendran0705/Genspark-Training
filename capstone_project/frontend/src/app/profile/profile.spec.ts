import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Profile } from './profile';
import { ProfileService } from '../services/profile-service';
import { of, throwError } from 'rxjs';

describe('Profile', () => {
  let component: Profile;
  let fixture: ComponentFixture<Profile>;
  let profileServiceSpy: jasmine.SpyObj<ProfileService>;

  beforeEach(async () => {
    profileServiceSpy = jasmine.createSpyObj('ProfileService', ['getUserProfile', 'setcustomername']);

    await TestBed.configureTestingModule({
      imports: [Profile],
      providers: [
        { provide: ProfileService, useValue: profileServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Profile);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch user profile and set ticket count on init', () => {
    spyOn(localStorage, 'getItem').and.returnValue('test@mail.com');
    const mockData = { name: 'Test', email: 'test@mail.com', tickets: [{}, {}] };
    profileServiceSpy.getUserProfile.and.returnValue(of(mockData));
    fixture.detectChanges();
    expect(profileServiceSpy.getUserProfile).toHaveBeenCalled();
    expect(component.userdata.name).toBe('Test');
    expect(component.ticketCount).toBe(2);
  });

  it('should handle error when fetching profile', () => {
    spyOn(localStorage, 'getItem').and.returnValue('test@mail.com');
    profileServiceSpy.getUserProfile.and.returnValue(throwError(() => new Error('Failed')));
    spyOn(console, 'error');
    fixture.detectChanges();
    expect(profileServiceSpy.getUserProfile).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to load profile:', jasmine.any(Error));
  });
});
