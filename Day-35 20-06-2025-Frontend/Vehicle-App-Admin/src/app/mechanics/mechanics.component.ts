import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-mechanics',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './mechanics.component.html',
  styleUrl: './mechanics.component.css'
})
export class MechanicsComponent implements OnInit {
  mechanics: any[] = [];
  errorMessage: string = '';

  showModal = false;
  searchTerm = '';
  allUsers: any[] = [];
  filteredUsers: any[] = [];
  // For delete confirmation
  showDeleteModal = false;
  mechanicToDelete: any = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadMechanics();
  }

  loadMechanics(): void {
    this.userService.getAllMechanics().subscribe({
      next: (response) => {
        this.mechanics = response;
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Error. Please try again later";
      }
    });
  }

  openModal() {
    this.showModal = true;
    this.userService.getAll().subscribe({
      next: (response) => {
        this.allUsers = response.filter((user: { isDeleted: any; roleName: string; }) => !user.isDeleted && user.roleName !== 'Mechanic' && user.roleName !== 'Admin');
        this.filteredUsers = [...this.allUsers];
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Unable to fetch users";
      }
    });
  }

  closeModal() {
    this.showModal = false;
    this.searchTerm = '';
    this.filteredUsers = [];
  }

  filterUsers() {
    const term = this.searchTerm.toLowerCase();
    this.filteredUsers = this.allUsers.filter(user => user.email.toLowerCase().includes(term));
  }

  makeMechanic(user: any) {
    const updatedUser = { UserId: user.id, RoleId: 2 };
    this.userService.updateRole(updatedUser).subscribe({
      next: () => {
        this.loadMechanics();
        this.closeModal();
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Could not update user role.";
      }
    });
  }

  confirmDelete(mechanic: any) {
    this.mechanicToDelete = mechanic;
    this.showDeleteModal = true;
  }

  closeDeleteModal() {
    this.mechanicToDelete = null;
    this.showDeleteModal = false;
  }

  removeMechanicConfirmed() {
    if (!this.mechanicToDelete) return;

    const updatedUser = { UserId: this.mechanicToDelete.id, RoleId: 3 };
    this.userService.updateRole(updatedUser).subscribe({
      next: () => {
        this.loadMechanics();
        this.closeDeleteModal();
      },
      error: (error) => {
        this.errorMessage = error.error.error ?? "Failed to update role";
      }
    });
  }
}
