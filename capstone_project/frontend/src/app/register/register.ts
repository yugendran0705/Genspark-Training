import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Loginservice } from '../services/loginservice';
import { Router } from '@angular/router';
import { CustomValidators } from '../validators/custom-validators';
import { RegisterInput } from '../models/RegisterInput';

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
        address: ['', Validators.required],
        walletBalance: [0, [Validators.required, Validators.min(0)]],
        role: ['user', Validators.required]
      },
      {
        validators: CustomValidators.matchPassword('password', 'confirmPassword')
      }
    );
  }

  checkEmail() {
    const email = this.registerForm.get('email')?.value;
    this.loginservice.checkUserExists(email).subscribe({
      next: () => {
        this.emailExists = true;
      },
      error: (err) => {
        if (err.status === 404) {
          this.emailExists = false;
          this.currentStep = 2;
        } else {
          console.error(err);
          alert("Error checking email.");
        }
      }
    });
  }

  onRegisterClick() {
    if (this.registerForm.valid) {
      const obj: RegisterInput = {
        name: this.registerForm.value.name,
        email: this.registerForm.value.email,
        password: this.registerForm.value.password,
        phoneNumber: this.registerForm.value.phone,
        address: this.registerForm.value.address,
        walletBalance: this.registerForm.value.walletBalance
      };

      this.loginservice.register(obj, this.registerForm.value.role).subscribe({
        next: () => {
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
