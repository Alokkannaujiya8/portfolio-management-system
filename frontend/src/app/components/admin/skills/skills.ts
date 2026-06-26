import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../services/portfolio';

@Component({
  selector: 'app-skills',
  standalone: false,
  templateUrl: './skills.html',
  styleUrl: './skills.css',
})
export class Skills implements OnInit {
  skills: any[] = [];
  isLoading = true;
  isSaving = false;

  // Active item model for Add/Edit
  model = {
    skillId: 0,
    skillName: '',
    percentage: 80,
    isActive: true
  };

  isEditMode = false;
  successMessage = '';
  errorMessage = '';

  constructor(private readonly portfolioService: Portfolio) {}

  ngOnInit(): void {
    this.loadSkills();
  }

  loadSkills(): void {
    this.isLoading = true;
    this.portfolioService.getSkills().subscribe({
      next: (data) => {
        this.skills = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching skills', err);
        this.isLoading = false;
      }
    });
  }

  onEdit(skill: any): void {
    this.isEditMode = true;
    this.model = { ...skill };
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
      this.portfolioService.updateSkill(this.model).subscribe({
        next: () => {
          this.successMessage = 'Skill updated successfully!';
          this.isSaving = false;
          this.isEditMode = false;
          this.resetForm();
          this.loadSkills();
        },
        error: (err) => {
          this.errorMessage = 'Failed to update skill.';
          this.isSaving = false;
        }
      });
    } else {
      this.portfolioService.addSkill(this.model).subscribe({
        next: () => {
          this.successMessage = 'Skill added successfully!';
          this.isSaving = false;
          this.resetForm();
          this.loadSkills();
        },
        error: (err) => {
          this.errorMessage = 'Failed to add skill.';
          this.isSaving = false;
        }
      });
    }
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this skill?')) {
      this.portfolioService.deleteSkill(id).subscribe({
        next: () => {
          this.successMessage = 'Skill deleted successfully!';
          this.loadSkills();
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete skill.';
        }
      });
    }
  }

  resetForm(): void {
    this.model = {
      skillId: 0,
      skillName: '',
      percentage: 80,
      isActive: true
    };
  }
}

