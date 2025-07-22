import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Loginservice } from '../services/loginservice';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit {

  showToast: boolean = false;

  constructor(
    public loginservice: Loginservice,private router:Router
  ) {
  }

  ngOnInit(): void {
    
  }

  handlelogout() {
    this.loginservice.logout();
    this.showToast = true;

    setTimeout(() => {
      this.showToast = false;
      this.router.navigate(['/login']);
    }, 2000);
  }

  closeToast() {
    this.showToast = false;
  }
}
