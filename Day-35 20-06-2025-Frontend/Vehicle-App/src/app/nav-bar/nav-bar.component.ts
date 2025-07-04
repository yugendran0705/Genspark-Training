import { Component, DoCheck } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  imports: [ 
    RouterModule,
    CommonModule, 
    ReactiveFormsModule
  ],
})
export class NavBarComponent implements DoCheck {
  isLoggedIn = false;
  mobileMenuOpen = false;

  constructor(private auth: AuthService) {}

  ngDoCheck() {
    this.isLoggedIn = !!localStorage.getItem('accessToken');
  }

}
