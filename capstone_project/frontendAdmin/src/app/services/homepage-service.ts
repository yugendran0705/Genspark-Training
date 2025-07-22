import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomepageService {



  constructor(private http: HttpClient) { }

  getallevents(): Observable<any> {
    const email=localStorage.getItem('username')
    return this.http.get<any>(`/api/events/admin/${email}`);
  }

}
