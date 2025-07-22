import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class InvoiceService{
    private apiURL = 'http://localhost:5192/api/v1/Invoice';

    constructor(private http: HttpClient) { }

    getAllInvoices(): Observable<any> {
        return this.http.get(`${this.apiURL}`, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
        });
    }

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

    createInvoice(body: {
        bookingId: number,
        amount: number,
        serviceDetails: string
    }): Observable<any> {
        return this.http.post(`${this.apiURL}`, body, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
        });
    }

    updateInvoice(id: number, body: {
        amount: number,
        serviceDetails: string
    }): Observable<any> {
        return this.http.put(`${this.apiURL}/${id}`, body, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
        });
    }

    deleteInvoice(id: number): Observable<any> {
        return this.http.delete(`${this.apiURL}/${id}`, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
        });
    }
}