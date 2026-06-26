import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private readonly apiUrl = `${API_BASE_URL}/api/Auth`;

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {}

  login(request: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, request).pipe(
      tap(res => {
        if (res && res.token) {
          localStorage.setItem('auth_token', res.token);
          localStorage.setItem('auth_user', JSON.stringify({
            username: res.username,
            email: res.email
          }));
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('auth_user');
    this.router.navigate(['/admin/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  getUser(): any {
    const user = localStorage.getItem('auth_user');
    return user ? JSON.parse(user) : null;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    // Quick JWT expiry check
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const isExpired = payload.exp * 1000 < Date.now();
      if (isExpired) {
        this.logout();
        return false;
      }
      return true;
    } catch {
      return false;
    }
  }
}

