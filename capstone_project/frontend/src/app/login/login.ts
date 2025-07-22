import { Component } from '@angular/core';
import { Loginservice } from '../services/loginservice';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  loginForm: FormGroup;
  showToast: boolean = false;

  constructor(private loginservice: Loginservice, private fb: FormBuilder, private router:Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onLoginClick() {
    console.log("Data-", this.loginForm.value);
    this.loginservice.login({username:this.loginForm.value.username, password:this.loginForm.value.password}).subscribe({
      next: (data) => {
        console.log(data);
        localStorage.setItem("token", data.token);
        localStorage.setItem("username", data.username);
        localStorage.setItem("role", data.role);
        

        this.loginservice.setlogintrue();
        this.loginservice.getuserdetails();
        this.showToast = true;
        
        setTimeout(() => {
          this.showToast = false;
          this.router.navigate(['/']);
        }, 2000);
      },
      error: (err) => {
        console.error('Login failed:', err.error);
        alert(`Login failed, ${err.error}`);
      }
    });
  }

  closeToast() {
    this.showToast = false;
  }
}

