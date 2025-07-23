import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  // Change the URL to match your backend API base URL
  private apiURL = 'http://localhost:5192/api/v1/Image';

  constructor(private http: HttpClient) { }

  getAllByBookingID(id: number): Observable<any> {
    return this.http.get(`${this.apiURL}/booking/${id}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  uploadImage(body: {
    Base64Data: string,
    BookingId: number,
    VehicleID: number
  }): Observable<any> {
    return this.http.post(`${this.apiURL}/`, body,{
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }

  deleteImage(id: number): Observable<any> {
    return this.http.delete(`${this.apiURL}/${id}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
    });
  }
}
