// slot.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SlotService {
  private apiURL = `${environment.apiBaseUrl}/api/v1/ServiceSlot`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<any> {
    return this.http.get(`${this.apiURL}/`, {
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

  getByMechanic(id: number): Observable<any> {
    return this.http.get(`${this.apiURL}/mechanic/${id}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  getAvailable():Observable<any> {
    return this.http.get(`${this.apiURL}/available`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  create(body: {
    slotDateTime: string,
    status: string,
    mechanicId: number
  }): Observable<any> {
    return this.http.post(`${this.apiURL}/`, body, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }  

  update(id: number, body: {
    slotDateTime: string,
    status: string,
    mechanicId: number
  }): Observable<any> {
    return this.http.put(`${this.apiURL}/${id}`, body, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiURL}/${id}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }
}
