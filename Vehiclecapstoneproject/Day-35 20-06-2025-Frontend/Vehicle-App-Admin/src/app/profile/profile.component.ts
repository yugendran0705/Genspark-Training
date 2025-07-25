import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { VehicleService } from '../services/vehicle.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  errorMessage: string = '';
  user: any = null;
  vehicles: any[] = [];
  isModalOpen: boolean = false;
  isModalDeleteOpen: boolean = false;
  vehicleForm: FormGroup;
  vehicleIdToDelete: number | null = null;
  yearOptions: number[] = [];

  constructor(
    private userService: UserService, 
    private fb: FormBuilder, 
    private vehicleService: VehicleService,
    private router: Router
  ) {
    // Build a reactive form to capture vehicle details.
    this.vehicleForm = this.fb.group({
      make: ['', Validators.required],
      model: ['', Validators.required],
      year: ['', [Validators.required, Validators.pattern('^[0-9]{4}$')]],
      registrationNumber: ['', Validators.required]
    });
  }

  ngOnInit(): void { 
    const currentYear = new Date().getFullYear();
    const startYear = 1980;
    for (let y = currentYear; y >= startYear; y--) {
      this.yearOptions.push(y);
    }
    this.userService.getOne().subscribe({
      next: (response) => {
        this.user = response;
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Error. Please try again later";
      }
    })
    this.vehicleService.getAllByUser().subscribe({
      next: (response) => {
        this.vehicles = response;
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Error. Please try again later";
      }
    })
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.router.navigate(['/']);
  }

  onAdd(): void {
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  onSubmitVehicle(): void {
    if (this.vehicleForm.valid) {
      this.vehicleService.create(this.vehicleForm.value).subscribe({
        next: (response)=>{
          this.vehicles.push(response);
        },
        error: (error) => {
          this.errorMessage = 'Vehicle is not added.';
        }
      })
      this.vehicleForm.reset();
      this.closeModal();
    } else {
      this.vehicleForm.markAllAsTouched();
    }
  }

  openDelete(id: number):void{
    this.vehicleIdToDelete = id;
    this.isModalDeleteOpen = true;
  }

  closeDelete():void{
    this.vehicleIdToDelete = null;
    this.isModalDeleteOpen = false;
  }

  deleteVehicle(): void {
    this.vehicleService.delete(this.vehicleIdToDelete).subscribe({
      next: () => {
        this.vehicles = this.vehicles.filter(v => v.id !== this.vehicleIdToDelete);
        this.closeDelete();
      },
      error: (error) => {
        this.errorMessage = 'Failed to delete vehicle.';
      }
    });
  }
}
