import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _token = signal<string | null>(localStorage.getItem('auth_token'));

  get token() {
    return this._token();
  }

  isLoggedIn(): boolean {
    return !!this._token();
  }

  login(token: string) {
    localStorage.setItem('auth_token', token);
    this._token.set(token);
  }

  logout() {
    localStorage.removeItem('auth_token');
    this._token.set(null);
  }
}
