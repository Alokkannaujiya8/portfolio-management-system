import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root',
})
export class Resume {
  private readonly apiUrl = `${API_BASE_URL}/api/Resume`;

  constructor(private readonly http: HttpClient) {}

  trackView(view: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/track`, view);
  }

  downloadResume(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/download`, { responseType: 'blob' });
  }

  getUserLocation(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/location`);
  }

  getLogs(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/logs`);
  }
}

