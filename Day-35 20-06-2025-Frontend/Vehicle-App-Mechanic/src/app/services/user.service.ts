// user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  // Change the URL to match your backend API base URL
  private apiURL = `${environment.apiBaseUrl}/api/v1/User`;

  constructor(private http: HttpClient) { }

  create(body: {
        name: string,
        email: string,
        phone: string,
        password: string
   }): Observable<any> {
    return this.http.post(`${this.apiURL}/`, body);
  }

  getAll(): Observable<any> {
    return this.http.get(`${this.apiURL}/`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getAllMechanics(): Observable<any> {
    return this.http.get(`${this.apiURL}/mechanics`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getOne(): Observable<any> {
    return this.http.get(`${this.apiURL}/profile`, 
      {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        },
        withCredentials: true
    });
  }

  update(body: {
        name: string,
        email: string,
        phone: string
   }): Observable<any> {
    return this.http.put(`${this.apiURL}/`, body,
      {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
      }
    );
  }

  delete(): Observable<any> {
    return this.http.delete(`${this.apiURL}/`, {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }
}
