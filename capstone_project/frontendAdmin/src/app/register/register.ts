import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Loginservice } from '../services/loginservice';
import { Router } from '@angular/router';
import { CustomValidators } from '../validators/custom-validators';

@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  registerForm: FormGroup;
  showToast: boolean = false;
  currentStep = 1;
  emailExists = false;

  constructor(
    private fb: FormBuilder,
    private loginservice: Loginservice,
    private router: Router
  ) {
    this.registerForm = this.fb.group(
      {
        name: ['', [Validators.required, Validators.minLength(3)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, CustomValidators.passwordStrength()]],
        confirmPassword: ['', Validators.required],
        phone: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
        role: ['user', Validators.required]
      },
      {
        validators: CustomValidators.matchPassword('password', 'confirmPassword')
      }
    );
  }

  // Step 1: Check email via API
  checkEmail() {
  const email = this.registerForm.get('email')?.value;
  this.loginservice.checkUserExists(email).subscribe({
    next: (user: any) => {
      // User exists
      this.emailExists = true;
    },
    error: (err) => {
      if (err.status === 404) {
        // User does not exist, move to next step
        this.emailExists = false;
        this.currentStep = 2;
      } else {
        // Other errors
        console.error(err);
        alert("Error checking email.");
      }
    }
  });
}

  // Step 2: Final registration
  onRegisterClick() {
    if (this.registerForm.valid) {
      const obj = {
        name: this.registerForm.value.name,
        email: this.registerForm.value.email,
        password: this.registerForm.value.password,
        phoneNumber: this.registerForm.value.phone
      };
      this.loginservice.register(obj, this.registerForm.value.role).subscribe({
        next: (data: any) => {
          this.showToast = true;
          setTimeout(() => {
            this.showToast = false;
            this.router.navigate(['/login']);
          }, 2000);
        },
        error: (err: any) => {
          alert(err.error);
        }
      });
    }
  }

  closeToast() {
    this.showToast = false;
  }
}
