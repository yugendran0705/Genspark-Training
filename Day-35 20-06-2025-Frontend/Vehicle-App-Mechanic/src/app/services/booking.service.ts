import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  // Change the URL to match your backend API base URL
  private apiURL = 'http://localhost:5192/api/v1/Booking';

  constructor(private http: HttpClient) { }

  getAllByUser(): Observable<any> {
    return this.http.get(`${this.apiURL}/user`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  create(body: {slotId: number, vehicleId: number}): Observable<any> {
    return this.http.post(`${this.apiURL}`, body, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getOne(id: number): Observable<any> {
    return this.http.get(`${this.apiURL}/${id}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getAllByMechanic(): Observable<any> {
    return this.http.get(`${this.apiURL}/mechanic`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getAll(): Observable<any> {
    return this.http.get(`${this.apiURL}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  updateStatus(id: number, status: string): Observable<any> {
    return this.http.put(`${this.apiURL}/${id}`, { status }, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }
}