import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { jwtDecode } from 'jwt-decode';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): Observable<boolean> {
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');

    if (!accessToken || !refreshToken) {
      this.router.navigate(['/login']);
      return of(false);
    }

    if (this.isTokenExpired(accessToken)) {
      return this.authService.refresh({ refreshToken: refreshToken }).pipe(
        map((response) => {
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
          return true;
        }),
        catchError(() => {
          this.router.navigate(['/login']);
          return of(false);
        })
      );
    }

    return of(true);
  }

  private isTokenExpired(token: string): boolean {
    try {
      const decoded: any = jwtDecode(token);
      const now = Math.floor(Date.now() / 1000);
      return decoded.exp < now;
    } catch (err) {
      return true;
    }
  }
}
