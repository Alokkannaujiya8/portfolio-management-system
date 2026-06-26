import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../services/portfolio';

@Component({
  selector: 'app-projects',
  standalone: false,
  templateUrl: './projects.html',
  styleUrl: './projects.css',
})
export class Projects implements OnInit {
  projects: any[] = [];
  isLoading = true;
  isSaving = false;

  model = {
    projectId: 0,
    projectName: '',
    description: '',
    gitHubLink: '',
    liveLink: '',
    imagePath: ''
  };

  imageFile: File | null = null;
  isEditMode = false;
  successMessage = '';
  errorMessage = '';

  constructor(private readonly portfolioService: Portfolio) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.isLoading = true;
    this.portfolioService.getProjects().subscribe({
      next: (data) => {
        this.projects = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching projects', err);
        this.isLoading = false;
      }
    });
  }

  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.imageFile = file;
    }
  }

  onEdit(proj: any): void {
    this.isEditMode = true;
    this.model = { ...proj };
    this.imageFile = null;
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

    const formData = new FormData();
    formData.append('projectId', this.model.projectId.toString());
    formData.append('projectName', this.model.projectName);
    formData.append('description', this.model.description);
    formData.append('gitHubLink', this.model.gitHubLink || '');
    formData.append('liveLink', this.model.liveLink || '');
    if (this.imageFile) {
      formData.append('imageFile', this.imageFile);
    }

    if (this.isEditMode) {
      this.portfolioService.updateProject(formData).subscribe({
        next: () => {
          this.successMessage = 'Project updated successfully!';
          this.isSaving = false;
          this.isEditMode = false;
          this.resetForm();
          this.loadProjects();
        },
        error: (err) => {
          this.errorMessage = 'Failed to update project.';
          this.isSaving = false;
        }
      });
    } else {
      this.portfolioService.addProject(formData).subscribe({
        next: () => {
          this.successMessage = 'Project added successfully!';
          this.isSaving = false;
          this.resetForm();
          this.loadProjects();
        },
        error: (err) => {
          this.errorMessage = 'Failed to add project.';
          this.isSaving = false;
        }
      });
    }
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this project?')) {
      this.portfolioService.deleteProject(id).subscribe({
        next: () => {
          this.successMessage = 'Project deleted successfully!';
          this.loadProjects();
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete project.';
        }
      });
    }
  }

  resetForm(): void {
    this.model = {
      projectId: 0,
      projectName: '',
      description: '',
      gitHubLink: '',
      liveLink: '',
      imagePath: ''
    };
    this.imageFile = null;
  }

  assetUrl(path?: string): string {
    if (!path) return '';
    return path.startsWith('http') ? path : `http://localhost:5154${path}`;
  }
}

