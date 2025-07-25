import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { BookTicketInput } from '../models/BookTicketInput';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  constructor(private httpclient:HttpClient) { }

  gettickets(email:string):Observable<any>{
    return this.httpclient.get(`http://localhost:5136/api/v1/ticket/gettickets/${email}`)
  }

  cancelTicket(id:number):Observable<any>{
    return this.httpclient.delete(`http://localhost:5136/api/v1/ticket/${id}/cancel`)
  }

  bookticket(input:BookTicketInput): Observable<any> {

    return this.httpclient.post(
      `http://localhost:5136/api/v1/ticket/book`,
      input,
      {
        responseType: 'blob'
      }
    );
  }
}