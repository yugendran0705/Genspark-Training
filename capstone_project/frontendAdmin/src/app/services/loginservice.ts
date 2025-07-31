import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class Loginservice {

  private apiUrl = 'http://localhost:5136/api/v1/authentication';
  public isloggedin = signal(false);

  constructor(private httpclient: HttpClient) {
    if (localStorage.getItem('token')) {
      this.isloggedin.update(value => true);
    }
  }


  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('role')
    localStorage.removeItem('username')
    this.isloggedin.update(value => false);
  }

  login(username: string, password: string): Observable<any> {
    const body = { username, password };
    return this.httpclient.post(this.apiUrl, body);
  }

  register(obj: any, role: string): Observable<any> {
    return this.httpclient.post('http://localhost:5136/api/v1/admin/register', obj);
  }


  checkUserExists(email: string): Observable<any> {
    return this.httpclient.get(`http://localhost:5136/api/v1/admin/${email}`);
  }
  
}
