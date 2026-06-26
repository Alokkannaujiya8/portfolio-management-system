import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root',
})
export class Visitor {
  private readonly apiUrl = `${API_BASE_URL}/api/Visitor`;
  private readonly dashboardUrl = `${API_BASE_URL}/api/Dashboard`;

  constructor(private readonly http: HttpClient) {}

  trackVisitor(visitor: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/track`, visitor);
  }

  trackPageVisit(visit: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/visit`, visit);
  }

  getLogs(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/logs`);
  }

  getPageVisits(visitorId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${visitorId}/visits`);
  }

  getDashboardStats(): Observable<any> {
    return this.http.get<any>(this.dashboardUrl);
  }
}

