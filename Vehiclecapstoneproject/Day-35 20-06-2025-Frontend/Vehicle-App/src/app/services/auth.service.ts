// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Change the URL to match your backend API base URL
  private apiURL = 'http://localhost:5192/api/v1/Auth';

  constructor(private http: HttpClient) { }

  login(credentials: { email: string, password: string }): Observable<any> {
    return this.http.post(`${this.apiURL}/login`, credentials);
  }

  refresh(body: { refreshToken: string }): Observable<any> {
    return this.http.post(`${this.apiURL}/refresh`, body);
  }
}
