import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../services/portfolio';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit {
  profile: any = {
    name: '',
    title: '',
    description: '',
    email: '',
    phone: '',
    address: '',
    linkedIn: '',
    gitHub: '',
    photo: '',
    resumePath: ''
  };

  photoFile: File | null = null;
  resumeFile: File | null = null;

  isLoading = true;
  isSaving = false;
  successMessage = '';
  errorMessage = '';

  constructor(private readonly portfolioService: Portfolio) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.portfolioService.getProfile().subscribe({
      next: (data) => {
        this.profile = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching profile', err);
        this.isLoading = false;
      }
    });
  }

  onPhotoChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.photoFile = file;
    }
  }

  onResumeChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.resumeFile = file;
    }
  }

  onSubmit(): void {
    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    const formData = new FormData();
    formData.append('profileId', this.profile.profileId?.toString() || '0');
    formData.append('name', this.profile.name || '');
    formData.append('title', this.profile.title || '');
    formData.append('description', this.profile.description || '');
    formData.append('email', this.profile.email || '');
    formData.append('phone', this.profile.phone || '');
    formData.append('address', this.profile.address || '');
    formData.append('linkedIn', this.profile.linkedIn || '');
    formData.append('gitHub', this.profile.gitHub || '');

    if (this.photoFile) {
      formData.append('photoFile', this.photoFile);
    }
    if (this.resumeFile) {
      formData.append('resumeFile', this.resumeFile);
    }

    this.portfolioService.updateProfile(formData).subscribe({
      next: (res) => {
        this.successMessage = 'Profile updated successfully!';
        this.isSaving = false;
        this.loadProfile(); // reload to get updated paths
        this.photoFile = null;
        this.resumeFile = null;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Error updating profile.';
        this.isSaving = false;
      }
    });
  }

  assetUrl(path?: string): string {
    if (!path) return '';
    return path.startsWith('http') ? path : `http://localhost:5154${path}`;
  }
}

