import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EventpageService {

  constructor(private httpclient:HttpClient) { }

  geteventbyid(id:number):Observable<any>
  {
    return this.httpclient.get(`/api/events/${id}`);
  }

  updateEvent(updatedevent:any,eventname:string):Observable<any>{
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.httpclient.put(`/api/events/${eventname}`,updatedevent,{headers:headers})
  }

  deleteEvent(eventname:any):Observable<any>{
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.httpclient.delete(`/api/events/${eventname}`,{headers:headers})
  }

  addevent(event:any):Observable<any>{
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.httpclient.post(`/api/events`,event,{headers:headers})
  }


}
