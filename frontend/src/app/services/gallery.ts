import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Gallery {
  private readonly apiUrl = 'http://localhost:5154/api/Gallery';

  constructor(private readonly http: HttpClient) {}

  // ==================== PUBLIC ENDPOINTS ====================

  getGalleryItems(type?: string): Observable<any[]> {
    const url = type ? `${this.apiUrl}?type=${type}` : this.apiUrl;
    return this.http.get<any[]>(url);
  }

  getCategories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/categories`);
  }

  getItemById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  incrementView(id: number): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}/view`, {});
  }

  incrementDownload(id: number): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}/download`, {});
  }

  // ==================== ADMIN ENDPOINTS ====================

  addItem(formData: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/admin`, formData);
  }

  updateItem(formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/admin`, formData);
  }

  deleteItem(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/admin/${id}`);
  }
}

