import { Component, OnInit } from '@angular/core';
import { Dashboardservice } from '../services/dashboardservice';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users.html',
  styleUrl: './users.css',
})
export class Users implements OnInit {
  users: any[] = [];

  filters = {
    name: '',
    age: null,
    gender: ''
  };

  constructor(private dashboardservice: Dashboardservice) {}

  ngOnInit(): void {
    this.dashboardservice.getallusers().subscribe({
      next: (data) => {
        this.users = data.users;
      },
      error: (err) => {
        console.error('Error fetching users:', err);
      },
    });
  }

  filteredUsers(): any[] {
    return this.users.filter((user) => {
      const matchesName =
        user.firstName.toLowerCase().includes(this.filters.name.toLowerCase()) 
      const matchesAge =
        !this.filters.age || user.age >= +this.filters.age;
      const matchesGender =
        !this.filters.gender || user.gender === this.filters.gender;
      return matchesName && matchesAge && matchesGender;
    });
  }
}
