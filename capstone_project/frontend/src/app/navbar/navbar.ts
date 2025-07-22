import { Component, HostListener, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Loginservice } from '../services/loginservice';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../services/profile-service';
import { NotificationService } from '../services/notification-service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit {

  showToast: boolean = false;
  islogged: boolean = false;
  name: string = "";
  showNotifications = false;


  notifications: any = [

  ];



  constructor(
    public loginservice: Loginservice,
    private notificationservice: NotificationService,
    private router:Router
  ) {
    this.loginservice.islogged$.subscribe((data: any) => this.islogged = data);
    this.loginservice.name$.subscribe((data: any) => this.name = data);
    this.notificationservice.updates$.subscribe((data: any) => {
      console.log("messages from navbar", data)
      this.notifications = [...data];
    });
  }

  ngOnInit(): void {

  }

  handlelogout() {
    this.loginservice.logout();
    this.showToast = true;

    setTimeout(() => {
      this.showToast = false;
      this.router.navigate(['/'])
    }, 2000);
  }

  closeToast() {
    this.showToast = false;
  }
  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }

  closeNotifications() {
    this.showNotifications = false;
  }

  removenotification(id:string){
    this.notificationservice.removeNotification(id);
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.nav-item.position-relative')) {
      this.showNotifications = false;
    }
  }



}
