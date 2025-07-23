import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfileComponent } from './profile.component';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { VehicleService } from '../services/vehicle.service';
import { UserService } from '../services/user.service';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let mockUserService: jasmine.SpyObj<UserService>;
  let mockVehicleService: jasmine.SpyObj<VehicleService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockUserService = jasmine.createSpyObj('UserService', ['getOne']);
    mockVehicleService = jasmine.createSpyObj('VehicleService', ['getAllByUser', 'create', 'delete']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [CommonModule, ReactiveFormsModule, ProfileComponent],
      providers: [
        { provide: UserService, useValue: mockUserService },
        { provide: VehicleService, useValue: mockVehicleService },
        { provide: Router, useValue: mockRouter },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize year options from current year to 1980', () => {
    const mockUser = { id: 1, name: 'Test User' };
    interface MockUser {
      id: number;
      name: string;
    }

    interface MockVehicle {
      id: number;
      make?: string;
      model?: string;
      year?: string;
      registrationNumber?: string;
    }

    const mockVehicles: MockVehicle[] = [];

    mockUserService.getOne.and.returnValue(of(mockUser));
    mockVehicleService.getAllByUser.and.returnValue(of(mockVehicles));

    component.ngOnInit();
    const currentYear = new Date().getFullYear();
    expect(component.yearOptions[0]).toBe(currentYear);
    expect(component.yearOptions.at(-1)).toBe(1980);
  });

  it('should load user and vehicles on init', () => {
    const mockUser = { id: 1, name: 'Test User' };
    const mockVehicles = [{ id: 1, make: 'Toyota' }];
    mockUserService.getOne.and.returnValue(of(mockUser));
    mockVehicleService.getAllByUser.and.returnValue(of(mockVehicles));

    component.ngOnInit();

    expect(component.user).toEqual(mockUser);
    expect(component.vehicles).toEqual(mockVehicles);
  });

  it('should set errorMessage when userService fails', () => {
    mockUserService.getOne.and.returnValue(throwError(() => ({ error: { error: 'User fetch failed' } })));
    mockVehicleService.getAllByUser.and.returnValue(of([]));

    component.ngOnInit();

    expect(component.errorMessage).toBe('User fetch failed');
  });

  it('should open and close add vehicle modal', () => {
    mockUserService.getOne.and.returnValue(of({ id: 1 }));
    mockVehicleService.getAllByUser.and.returnValue(of([]));

    component.ngOnInit(); // initializes yearOptions and form
    expect(component.isModalOpen).toBeFalse();
    component.onAdd();
    expect(component.isModalOpen).toBeTrue();
    component.closeModal();
    expect(component.isModalOpen).toBeFalse();
  });


  it('should open and close delete modal', () => {
    expect(component.isModalDeleteOpen).toBeFalse();
    component.openDelete(2);
    expect(component.isModalDeleteOpen).toBeTrue();
    expect(component.vehicleIdToDelete).toBe(2);
    component.closeDelete();
    expect(component.isModalDeleteOpen).toBeFalse();
    expect(component.vehicleIdToDelete).toBeNull();
  });

  it('should logout and navigate to root', () => {
    localStorage.setItem('accessToken', 'abc');
    localStorage.setItem('refreshToken', 'xyz');

    component.logout();

    expect(localStorage.getItem('accessToken')).toBeNull();
    expect(localStorage.getItem('refreshToken')).toBeNull();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/']);
  });

  it('should add a vehicle when form is valid', () => {
    mockUserService.getOne.and.returnValue(of({ id: 1 }));
    mockVehicleService.getAllByUser.and.returnValue(of([]));
    mockVehicleService.create.and.returnValue(of({ id: 1, make: 'Honda' }));

    component.ngOnInit();

    component.vehicleForm.setValue({
      make: 'Honda',
      model: 'Civic',
      year: '2022',
      registrationNumber: 'ABC123'
    });

    component.onSubmitVehicle();

    expect(component.vehicles.length).toBe(1);
    expect(component.vehicles[0].make).toBe('Honda');
    expect(component.isModalOpen).toBeFalse();
  });


  it('should not submit vehicle form if invalid', () => {
    mockUserService.getOne.and.returnValue(of({ id: 1 }));
    mockVehicleService.getAllByUser.and.returnValue(of([]));

    component.ngOnInit();
    component.vehicleForm.setValue({
      make: '',
      model: '',
      year: '',
      registrationNumber: ''
    });

    component.onSubmitVehicle();

    expect(component.vehicles.length).toBe(0);
    expect(component.isModalOpen).toBeFalse();
  });


  it('should handle vehicle delete', () => {
    const vehicleId = 5;
    component.vehicles = [{ id: 5 }, { id: 6 }];
    component.vehicleIdToDelete = vehicleId;
    mockVehicleService.delete.and.returnValue(of({}));

    component.deleteVehicle();

    expect(component.vehicles.length).toBe(1);
    expect(component.vehicles.find(v => v.id === 5)).toBeUndefined();
    expect(component.isModalDeleteOpen).toBeFalse();
  });

  it('should handle vehicle delete error', () => {
    component.vehicleIdToDelete = 99;
    component.vehicles = [{ id: 99 }];
    mockVehicleService.delete.and.returnValue(throwError(() => ({ error: {} })));

    component.deleteVehicle();

    expect(component.errorMessage).toBe('Failed to delete vehicle.');
  });
});
