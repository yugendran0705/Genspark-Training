import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private httpclient:HttpClient) { }

  geteventbyid(id:number):Observable<any>
  {
    return this.httpclient.get(`/api/events/${id}`);
  }

  getallevents(): Observable<any> {
    return this.httpclient.get<any>('/api/events');
  }
}
