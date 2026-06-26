import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable } from 'rxjs';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root',
})
export class Portfolio {
  private readonly apiUrl = `${API_BASE_URL}/api`;

  constructor(private readonly http: HttpClient) {}

  getHomeData(): Observable<HomeData> {
    return forkJoin({
      profile: this.http.get<Profile>(`${this.apiUrl}/Profile`),
      skills: this.http.get<Skill[]>(`${this.apiUrl}/Skills`),
      projects: this.http.get<Project[]>(`${this.apiUrl}/Projects`),
      experience: this.http.get<Experience[]>(`${this.apiUrl}/Experience`),
      education: this.http.get<Education[]>(`${this.apiUrl}/Education`)
    });
  }

  getProfile(): Observable<Profile> {
    return this.http.get<Profile>(`${this.apiUrl}/Profile`);
  }

  updateProfile(formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Profile`, formData);
  }

  getSkills(): Observable<Skill[]> {
    return this.http.get<Skill[]>(`${this.apiUrl}/Skills`);
  }

  addSkill(skill: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Skills`, skill);
  }

  updateSkill(skill: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Skills`, skill);
  }

  deleteSkill(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Skills/${id}`);
  }

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.apiUrl}/Projects`);
  }

  addProject(formData: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Projects`, formData);
  }

  updateProject(formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Projects`, formData);
  }

  deleteProject(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Projects/${id}`);
  }

  getExperiences(): Observable<Experience[]> {
    return this.http.get<Experience[]>(`${this.apiUrl}/Experience`);
  }

  addExperience(exp: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Experience`, exp);
  }

  updateExperience(exp: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Experience`, exp);
  }

  deleteExperience(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Experience/${id}`);
  }

  getEducation(): Observable<Education[]> {
    return this.http.get<Education[]>(`${this.apiUrl}/Education`);
  }

  addEducation(edu: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Education`, edu);
  }

  updateEducation(edu: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Education`, edu);
  }

  deleteEducation(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Education/${id}`);
  }

  sendMessage(message: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Contact/send`, message);
  }

  getMessages(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Contact/messages`);
  }

  markMessageAsRead(id: number): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Contact/messages/${id}/read`, {});
  }
}


export interface HomeData {
  profile: Profile;
  skills: Skill[];
  projects: Project[];
  experience: Experience[];
  education: Education[];
}

export interface Profile {
  profileId: number;
  name: string;
  title: string;
  description: string;
  email: string;
  phone: string;
  address: string;
  linkedIn?: string;
  gitHub?: string;
  photo?: string;
  resumePath?: string;
}

export interface Skill {
  skillId: number;
  skillName: string;
  percentage: number;
}

export interface Project {
  projectId: number;
  projectName: string;
  description: string;
  imagePath?: string;
  gitHubLink?: string;
  liveLink?: string;
}

export interface Experience {
  experienceId: number;
  companyName: string;
  role: string;
  startDate: string;
  endDate?: string;
  description?: string;
}

export interface Education {
  educationId: number;
  degree: string;
  institute: string;
  year: number;
  percentage?: number;
}
