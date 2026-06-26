import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Blog {
  private readonly apiUrl = 'http://localhost:5154/api/Blog';

  constructor(private readonly http: HttpClient) {}

  // ==================== PUBLIC METHODS ====================

  getPublishedPosts(params: {
    categoryId?: number;
    tag?: string;
    search?: string;
    page?: number;
    pageSize?: number;
  }): Observable<any> {
    let httpParams = new HttpParams();
    if (params.categoryId) httpParams = httpParams.set('categoryId', params.categoryId.toString());
    if (params.tag) httpParams = httpParams.set('tag', params.tag);
    if (params.search) httpParams = httpParams.set('search', params.search);
    if (params.page) httpParams = httpParams.set('page', params.page.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());

    return this.http.get<any>(`${this.apiUrl}/posts`, { params: httpParams });
  }

  getPostBySlug(slug: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/posts/${slug}`);
  }

  getCategories(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/categories`);
  }

  getPopularPosts(count = 5): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/popular?count=${count}`);
  }

  getRelatedPosts(id: number, categoryId: number, count = 3): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/posts/${id}/related?categoryId=${categoryId}&count=${count}`);
  }

  toggleLike(id: number, visitorId: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/posts/${id}/like?visitorId=${visitorId}`, {});
  }

  getComments(id: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/posts/${id}/comments`);
  }

  addComment(id: number, comment: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/posts/${id}/comment`, comment);
  }

  // ==================== ADMIN METHODS ====================

  adminGetPosts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/admin/posts`);
  }

  adminGetPostById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/admin/posts/${id}`);
  }

  addPost(formData: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/admin/posts`, formData);
  }

  updatePost(formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/admin/posts`, formData);
  }

  deletePost(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/admin/posts/${id}`);
  }

  adminAddCategory(category: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/admin/categories`, category);
  }

  adminDeleteCategory(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/admin/categories/${id}`);
  }

  adminGetComments(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/admin/comments`);
  }

  adminApproveComment(id: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/admin/comments/${id}/approve`, {});
  }

  adminDeleteComment(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/admin/comments/${id}`);
  }
}

