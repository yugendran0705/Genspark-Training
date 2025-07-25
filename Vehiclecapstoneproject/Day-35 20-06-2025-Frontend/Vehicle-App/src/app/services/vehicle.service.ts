// user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  // Change the URL to match your backend API base URL
  private apiURL = 'http://localhost:5192/api/v1/Vehicle';

  constructor(private http: HttpClient) { }

  create(body: {
        make: string,
        model: string,
        year: number,
        registrationNumber: string
   }): Observable<any> {
    return this.http.post(`${this.apiURL}/`, body,{
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getAll(): Observable<any> {
    return this.http.get(`${this.apiURL}/`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getOne(id: any): Observable<any> {
    return this.http.get(`${this.apiURL}/${id}`, 
      {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        },
        withCredentials: true
    });
  }

  update(id: any, body: {
        make: string,
        model: string,
        year: number,
        registrationNumber: string
   }): Observable<any> {
    return this.http.put(`${this.apiURL}/${id}`, body,
      {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
      }
    );
  }

  delete(id: any): Observable<any> {
    return this.http.delete(`${this.apiURL}/${id}`, {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getAllByUser(): Observable<any> {
    return this.http.get(`${this.apiURL}/user`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }
}
