import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Wallet {
  id: number;
  userId: number;
  balance: number;
  email: string;
}

export interface WalletTransactionDto {
  email: string;
  amount: number;
}

@Injectable({
  providedIn: 'root'
})
export class WalletService {

  private baseUrl = 'http://localhost:5136/api/Wallet';

  constructor(private http: HttpClient) { }

  // GET /api/Wallet/{email}
  getWalletByEmail(email: string): Observable<Wallet> {
    return this.http.get<Wallet>(`${this.baseUrl}/${email}`);
  }

  // PUT /api/Wallet/add
  addFunds(dto: WalletTransactionDto): Observable<Wallet> {
    return this.http.put<Wallet>(`${this.baseUrl}/add`, dto);
  }

  // PUT /api/Wallet/deduct
  deductFunds(dto: WalletTransactionDto): Observable<Wallet> {
    return this.http.put<Wallet>(`${this.baseUrl}/deduct`, dto);
  }
}
