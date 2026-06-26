import { Component, OnInit } from '@angular/core';
import { Resume } from '../../../services/resume';

@Component({
  selector: 'app-resume-download',
  standalone: false,
  templateUrl: './resume-download.html',
  styleUrl: './resume-download.css',
})
export class ResumeDownload implements OnInit {
  model = {
    visitorName: '',
    visitorEmail: '',
    companyName: '',
    designation: '',
    country: '',
    city: '',
  };
  isLoading = false;
  errorMessage = '';

  constructor(private readonly resumeService: Resume) {}

  ngOnInit(): void {
    this.resumeService.getUserLocation().subscribe({
      next: (data) => {
        if (data.country) this.model.country = data.country;
        if (data.city) this.model.city = data.city;
      },
      error: () => console.log('Location could not be loaded.')
    });
  }

  onSubmit(): void {
    if (!this.model.visitorName || !this.model.visitorEmail) {
      this.errorMessage = 'Please fill out all required fields.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    // First track the download
    this.resumeService.trackView(this.model).subscribe({
      next: () => {
        // Then start the file stream download
        this.resumeService.downloadResume().subscribe({
          next: (blob) => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'Resume.pdf';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
            this.isLoading = false;
            
            // Clear form
            this.model.visitorName = '';
            this.model.visitorEmail = '';
            this.model.companyName = '';
            this.model.designation = '';
          },
          error: (err) => {
            this.errorMessage = 'Resume file download failed.';
            this.isLoading = false;
          }
        });
      },
      error: (err) => {
        this.errorMessage = 'Failed to log download tracking.';
        this.isLoading = false;
      }
    });
  }
}

