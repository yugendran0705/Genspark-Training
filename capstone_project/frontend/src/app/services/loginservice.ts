import { Injectable, signal } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { RegisterInput } from '../models/RegisterInput';
import { LoginInput } from '../models/LoginInput';

@Injectable({
  providedIn: 'root'
})
export class Loginservice {

  private apiUrl = '/api/v1/authentication';

  private loginsubject = new BehaviorSubject<boolean>(false);
  islogged$ = this.loginsubject.asObservable();

  private namesubject=new BehaviorSubject<string>("");
  name$=this.namesubject.asObservable();


  constructor(private httpclient: HttpClient) {
    if (localStorage.getItem('token')) {
      this.loginsubject.next(true);
    }
  }


  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('role')
    localStorage.removeItem('username')
    this.loginsubject.next(false);
  }

  login(logindata:LoginInput): Observable<any> {
    const body = logindata;
    return this.httpclient.post(this.apiUrl, body);
  }

  register(obj: RegisterInput, role: string): Observable<any> {
    if (role == "user") {
      return this.httpclient.post('/api/v1/customer/register', obj);
    }
    return this.httpclient.post('/api/v1/admin/register', obj);
  }

  setlogintrue() {
    this.loginsubject.next(true);
  }

  getuserdetails(){
    const token = localStorage.getItem('token');
    const email= localStorage.getItem('username');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    const resp= this.httpclient.get(`/api/v1/customer/${email}`, {headers:headers});
    resp.subscribe((data:any)=>{
      console.log(data)
      this.namesubject.next(data.name);
    });
  }

  checkUserExists(email:string):Observable<any>{
    return this.httpclient.get(`/api/v1/customer/${email}`);
  }


}
