import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../services/portfolio';

@Component({
  selector: 'app-education',
  standalone: false,
  templateUrl: './education.html',
  styleUrl: './education.css',
})
export class Education implements OnInit {
  educations: any[] = [];
  isLoading = true;
  isSaving = false;

  model = {
    educationId: 0,
    degree: '',
    institute: '',
    year: new Date().getFullYear(),
    percentage: 80,
    isActive: true
  };

  isEditMode = false;
  successMessage = '';
  errorMessage = '';

  constructor(private readonly portfolioService: Portfolio) {}

  ngOnInit(): void {
    this.loadEducation();
  }

  loadEducation(): void {
    this.isLoading = true;
    this.portfolioService.getEducation().subscribe({
      next: (data) => {
        this.educations = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching education records', err);
        this.isLoading = false;
      }
    });
  }

  onEdit(edu: any): void {
    this.isEditMode = true;
    this.model = { ...edu };
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

    if (this.isEditMode) {
      this.portfolioService.updateEducation(this.model).subscribe({
        next: () => {
          this.successMessage = 'Education record updated successfully!';
          this.isSaving = false;
          this.isEditMode = false;
          this.resetForm();
          this.loadEducation();
        },
        error: (err) => {
          this.errorMessage = 'Failed to update education record.';
          this.isSaving = false;
        }
      });
    } else {
      this.portfolioService.addEducation(this.model).subscribe({
        next: () => {
          this.successMessage = 'Education record added successfully!';
          this.isSaving = false;
          this.resetForm();
          this.loadEducation();
        },
        error: (err) => {
          this.errorMessage = 'Failed to add education record.';
          this.isSaving = false;
        }
      });
    }
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this education record?')) {
      this.portfolioService.deleteEducation(id).subscribe({
        next: () => {
          this.successMessage = 'Education record deleted successfully!';
          this.loadEducation();
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete education record.';
        }
      });
    }
  }

  resetForm(): void {
    this.model = {
      educationId: 0,
      degree: '',
      institute: '',
      year: new Date().getFullYear(),
      percentage: 80,
      isActive: true
    };
  }
}

