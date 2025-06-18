import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Dashboardservice {

  private http = inject(HttpClient);

  constructor() { }

  getallusers():Observable<any> {
    return this.http.get<any[]>('https://dummyjson.com/users');
  }
}
