import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../services/portfolio';

@Component({
  selector: 'app-experience',
  standalone: false,
  templateUrl: './experience.html',
  styleUrl: './experience.css',
})
export class Experience implements OnInit {
  experiences: any[] = [];
  isLoading = true;
  isSaving = false;

  model = {
    experienceId: 0,
    companyName: '',
    role: '',
    startDate: '',
    endDate: '',
    description: '',
    isActive: true
  };

  isEditMode = false;
  successMessage = '';
  errorMessage = '';

  constructor(private readonly portfolioService: Portfolio) {}

  ngOnInit(): void {
    this.loadExperiences();
  }

  loadExperiences(): void {
    this.isLoading = true;
    this.portfolioService.getExperiences().subscribe({
      next: (data) => {
        this.experiences = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching experiences', err);
        this.isLoading = false;
      }
    });
  }

  onEdit(exp: any): void {
    this.isEditMode = true;
    
    // Format dates to YYYY-MM-DD for HTML input field compatibility
    const start = exp.startDate ? exp.startDate.split('T')[0] : '';
    const end = exp.endDate ? exp.endDate.split('T')[0] : '';

    this.model = { 
      ...exp,
      startDate: start,
      endDate: end
    };
    
    this.successMessage = '';
    this.errorMessage = '';
  }

  onCancel(): void {
    this.isEditMode = false;
    this.resetForm();
  }

  onSubmit(): void {
    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    // Handle optional empty string for endDate
    const payload = {
      ...this.model,
      endDate: this.model.endDate === '' ? null : this.model.endDate
    };

    if (this.isEditMode) {
      this.portfolioService.updateExperience(payload).subscribe({
        next: () => {
          this.successMessage = 'Experience updated successfully!';
          this.isSaving = false;
          this.isEditMode = false;
          this.resetForm();
          this.loadExperiences();
        },
        error: (err) => {
          this.errorMessage = 'Failed to update experience.';
          this.isSaving = false;
        }
      });
    } else {
      this.portfolioService.addExperience(payload).subscribe({
        next: () => {
          this.successMessage = 'Experience added successfully!';
          this.isSaving = false;
          this.resetForm();
          this.loadExperiences();
        },
        error: (err) => {
          this.errorMessage = 'Failed to add experience.';
          this.isSaving = false;
        }
      });
    }
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this experience record?')) {
      this.portfolioService.deleteExperience(id).subscribe({
        next: () => {
          this.successMessage = 'Experience deleted successfully!';
          this.loadExperiences();
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete experience.';
        }
      });
    }
  }

  resetForm(): void {
    this.model = {
      experienceId: 0,
      companyName: '',
      role: '',
      startDate: '',
      endDate: '',
      description: '',
      isActive: true
    };
  }
}

