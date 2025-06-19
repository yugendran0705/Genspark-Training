import { Component, ElementRef, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { User } from '../models/user.model';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.css'],
  imports:[CommonModule, ReactiveFormsModule]
})
export class UserSearchComponent implements OnInit, AfterViewInit {
  @ViewChild('searchInput') searchInput!: ElementRef;
  filteredUsers: User[] = [];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    // Initialize with all users
    this.userService.getUsers().subscribe(users => {
      this.filteredUsers = users;
    });
  }

  ngAfterViewInit(): void {
    fromEvent<Event>(this.searchInput.nativeElement, 'input').pipe(
      debounceTime(300),
      map((event: Event) => (event.target as HTMLInputElement).value),
      distinctUntilChanged(),
      switchMap((query: string) => this.userService.filterUsers(query))
    ).subscribe((users: User[]) => {
      this.filteredUsers = users;
    });
  }
}
