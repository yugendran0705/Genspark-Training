import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class InvoiceService{
    private apiURL = 'http://localhost:5192/api/v1/Invoice';

    constructor(private http: HttpClient) { }

    getInvoiceByBookingID(bookingId: string): Observable<any> {
        return this.http.get(`${this.apiURL}/booking/${bookingId}`, {
            headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
        });
    }
    getInvoicePDFByBookingID(bookingId: string): Observable<any> {
        return this.http.get(`${this.apiURL}/booking/${bookingId}/pdf`, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            },
            responseType: 'blob'
        });
    }
}