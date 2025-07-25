import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UpdateUserProfileInput } from '../models/UpdateUserProfileInput';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private httpclient:HttpClient){ }

  public customername=signal<any>(null)

  setcustomername(name:string){
    this.customername.set(name);
  }



  getUserProfile():Observable<any>{
    const email=localStorage.getItem('username');
    return this.httpclient.get(`http://localhost:5136/api/v1/customer/${email}`)
  }

  updateUserProfile(updatedData:UpdateUserProfileInput):Observable<any>{
    return this.httpclient.put(`http://localhost:5136/api/v1/customer/update`,updatedData)
  }
}
