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
  vehicleForm: FormGroup;

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
    this.userService.getOne().subscribe({
      next: (response) => {
        this.user = response;
      },
      error: (error) => {
        this.errorMessage = error.error ?? "Error. Please try again later";
      }
    })
    this.vehicleService.getAllByUser().subscribe({
      next: (response) => {
        console.log(response);
        this.vehicles = response;
      },
      error: (error) => {
        this.errorMessage = error.error ?? "Error. Please try again later";
      }
    })
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.router.navigate(['/']);
  }

  // Called when the plus button is clicked – opens the modal.
  onAdd(): void {
    this.isModalOpen = true;
  }

  // Closes the modal.
  closeModal(): void {
    this.isModalOpen = false;
  }

  // Called when the vehicle form is submitted.
  onSubmitVehicle(): void {
    if (this.vehicleForm.valid) {
      // Process your vehicle data here (for example, call a vehicle service).
      console.log('Vehicle Details:', this.vehicleForm.value);
      this.vehicleService.create(this.vehicleForm.value).subscribe({
        next: (response)=>{
          console.log(response);
        },
        error: (error) => {
          this.errorMessage = 'Vehicle is not added.';
        }
      })
      this.vehicleForm.reset();
      this.closeModal();
    } else {
      // Mark all as touched to trigger validation errors display.
      this.vehicleForm.markAllAsTouched();
    }
  }
}
