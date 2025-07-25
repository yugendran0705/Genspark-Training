import { Component, DoCheck } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
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

  constructor() {}

  ngDoCheck() {
    this.isLoggedIn = !!localStorage.getItem('accessToken');
  }

}
