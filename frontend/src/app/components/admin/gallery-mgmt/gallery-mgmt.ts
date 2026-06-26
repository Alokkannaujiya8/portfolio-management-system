import { Component, OnInit } from '@angular/core';
import { Gallery } from '../../../services/gallery';

@Component({
  selector: 'app-gallery-mgmt',
  standalone: false,
  templateUrl: './gallery-mgmt.html',
  styleUrl: './gallery-mgmt.css',
})
export class GalleryMgmt implements OnInit {
  items: any[] = [];
  categories: string[] = [];
  isLoading = true;
  isSaving = false;

  showForm = false;
  isEditMode = false;

  model = {
    galleryId: 0,
    title: '',
    description: '',
    mediaType: 'Image', // 'Image' | 'Video' | 'Document'
    category: '',
    tags: '',
    displayOrder: 0,
    isFeatured: false,
    videoEmbedCode: '',
    isActive: true
  };

  selectedImageFile: File | null = null;
  selectedDocFile: File | null = null;
  imagePreview: string | null = null;
  
  successMessage = '';
  errorMessage = '';

  constructor(private readonly galleryService: Gallery) {}

  ngOnInit(): void {
    this.loadItems();
    this.loadCategories();
  }

  loadItems(): void {
    this.isLoading = true;
    this.galleryService.getGalleryItems().subscribe({
      next: (data) => {
        this.items = data.sort((a, b) => a.displayOrder - b.displayOrder);
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching gallery items', err);
        this.isLoading = false;
      }
    });
  }

  loadCategories(): void {
    this.galleryService.getCategories().subscribe({
      next: (data) => this.categories = data,
      error: (err) => console.error('Error loading gallery categories', err)
    });
  }

  onAddNew(): void {
    this.isEditMode = false;
    this.showForm = true;
    this.resetForm();
  }

  onEdit(item: any): void {
    this.isEditMode = true;
    this.showForm = true;
    this.successMessage = '';
    this.errorMessage = '';
    this.model = {
      galleryId: item.galleryId,
      title: item.title || '',
      description: item.description || '',
      mediaType: item.mediaType || 'Image',
      category: item.category || '',
      tags: item.tags || '',
      displayOrder: item.displayOrder || 0,
      isFeatured: item.isFeatured || false,
      videoEmbedCode: item.videoEmbedCode || '',
      isActive: item.isActive ?? true
    };
    this.selectedImageFile = null;
    this.selectedDocFile = null;
    if (item.mediaType === 'Image' && item.mediaPath) {
      this.imagePreview = `http://localhost:5154${item.mediaPath}`;
    } else {
      this.imagePreview = null;
    }
  }

  onCancel(): void {
    this.showForm = false;
    this.resetForm();
  }

  onImageFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedImageFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  onDocFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedDocFile = file;
    }
  }

  onSubmit(): void {
    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    const formData = new FormData();
    formData.append('GalleryId', this.model.galleryId.toString());
    formData.append('Title', this.model.title);
    formData.append('Description', this.model.description);
    formData.append('MediaType', this.model.mediaType);
    formData.append('Category', this.model.category);
    formData.append('Tags', this.model.tags);
    formData.append('DisplayOrder', this.model.displayOrder.toString());
    formData.append('IsFeatured', this.model.isFeatured.toString());
    formData.append('VideoEmbedCode', this.model.videoEmbedCode);
    formData.append('IsActive', this.model.isActive.toString());

    if (this.selectedImageFile) {
      formData.append('ImageFile', this.selectedImageFile);
    }
    if (this.selectedDocFile) {
      formData.append('DocumentFile', this.selectedDocFile);
    }

    if (this.isEditMode) {
      this.galleryService.updateItem(formData).subscribe({
        next: () => {
          this.successMessage = 'Gallery item updated successfully!';
          this.isSaving = false;
          this.showForm = false;
          this.resetForm();
          this.loadItems();
          this.loadCategories();
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Failed to update gallery item.';
          this.isSaving = false;
        }
      });
    } else {
      this.galleryService.addItem(formData).subscribe({
        next: () => {
          this.successMessage = 'Gallery item added successfully!';
          this.isSaving = false;
          this.showForm = false;
          this.resetForm();
          this.loadItems();
          this.loadCategories();
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Failed to add gallery item.';
          this.isSaving = false;
        }
      });
    }
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this gallery item?')) {
      this.galleryService.deleteItem(id).subscribe({
        next: () => {
          this.successMessage = 'Gallery item deleted successfully!';
          this.loadItems();
          this.loadCategories();
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete gallery item.';
          console.error(err);
        }
      });
    }
  }

  resetForm(): void {
    this.model = {
      galleryId: 0,
      title: '',
      description: '',
      mediaType: 'Image',
      category: '',
      tags: '',
      displayOrder: 0,
      isFeatured: false,
      videoEmbedCode: '',
      isActive: true
    };
    this.selectedImageFile = null;
    this.selectedDocFile = null;
    this.imagePreview = null;
  }
}
