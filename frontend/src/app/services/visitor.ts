import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Visitor {
  private readonly apiUrl = 'http://localhost:5154/api/Visitor';
  private readonly dashboardUrl = 'http://localhost:5154/api/Dashboard';

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

