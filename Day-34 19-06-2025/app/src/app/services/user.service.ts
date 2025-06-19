import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersSubject: BehaviorSubject<User[]> = new BehaviorSubject<User[]>([]);

  constructor() { }

  getUsers(): Observable<User[]> {
    return this.usersSubject.asObservable();
  }

  addUser(user: User): void {
    const currentUsers = this.usersSubject.getValue();
    this.usersSubject.next([...currentUsers, user]);
  }

  filterUsers(query: string): Observable<User[]> {
    const lowercaseQuery = query.toLowerCase();
    return new Observable(subscriber => {
      const filtered = this.usersSubject.getValue().filter(u =>
        u.username.toLowerCase().includes(lowercaseQuery) ||
        u.role.toLowerCase().includes(lowercaseQuery)
      );
      subscriber.next(filtered);
      subscriber.complete();
    });
  }
}
