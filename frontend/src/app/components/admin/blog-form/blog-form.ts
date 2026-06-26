import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Blog } from '../../../services/blog';
import { API_BASE_URL } from '../../../config';

@Component({
  selector: 'app-blog-form',
  standalone: false,
  templateUrl: './blog-form.html',
  styleUrl: './blog-form.css',
})
export class BlogForm implements OnInit {
  isEditMode = false;
  isLoading = false;
  isSaving = false;
  postId = 0;
  
  categories: any[] = [];
  selectedFile: File | null = null;
  imagePreview: string | null = null;

  model = {
    postId: 0,
    title: '',
    slug: '',
    excerpt: '',
    content: '',
    categoryId: null as number | null,
    tags: '',
    metaTitle: '',
    metaDescription: '',
    metaKeywords: '',
    isFeatured: false,
    isPublished: true
  };

  successMessage = '';
  errorMessage = '';

  constructor(
    private readonly blogService: Blog,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.loadCategories();

    this.route.paramMap.subscribe(params => {
      const idStr = params.get('id');
      if (idStr) {
        this.isEditMode = true;
        this.postId = parseInt(idStr);
        this.model.postId = this.postId;
        this.loadPostDetails();
      }
    });
  }

  loadCategories(): void {
    this.blogService.getCategories().subscribe({
      next: (data) => this.categories = data,
      error: (err) => console.error('Error fetching categories', err)
    });
  }

  loadPostDetails(): void {
    this.isLoading = true;
    this.blogService.adminGetPostById(this.postId).subscribe({
      next: (post) => {
        this.model = {
          postId: post.postId,
          title: post.title || '',
          slug: post.slug || '',
          excerpt: post.excerpt || '',
          content: post.content || '',
          categoryId: post.categoryId || null,
          tags: post.tags || '',
          metaTitle: post.metaTitle || '',
          metaDescription: post.metaDescription || '',
          metaKeywords: post.metaKeywords || '',
          isFeatured: post.isFeatured || false,
          isPublished: post.isPublished ?? true
        };
        if (post.featuredImage) {
          this.imagePreview = `${API_BASE_URL}${post.featuredImage}`;
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading post details', err);
        this.errorMessage = 'Failed to load blog post details.';
        this.isLoading = false;
      }
    });
  }

  onTitleChange(): void {
    if (!this.isEditMode) {
      this.model.slug = this.model.title
        .toLowerCase()
        .replace(/[^a-z0-9 -]/g, '') // remove invalid chars
        .replace(/\s+/g, '-') // collapse whitespace and replace by -
        .replace(/-+/g, '-'); // collapse dashes
    }
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    const formData = new FormData();
    formData.append('postId', this.model.postId.toString());
    formData.append('title', this.model.title);
    formData.append('slug', this.model.slug);
    formData.append('excerpt', this.model.excerpt);
    formData.append('content', this.model.content);
    if (this.model.categoryId) {
      formData.append('categoryId', this.model.categoryId.toString());
    }
    formData.append('tags', this.model.tags);
    formData.append('metaTitle', this.model.metaTitle);
    formData.append('metaDescription', this.model.metaDescription);
    formData.append('metaKeywords', this.model.metaKeywords);
    formData.append('isFeatured', this.model.isFeatured.toString());
    formData.append('isPublished', this.model.isPublished.toString());

    if (this.selectedFile) {
      formData.append('FeaturedImageFile', this.selectedFile);
    }

    if (this.isEditMode) {
      this.blogService.updatePost(formData).subscribe({
        next: () => {
          this.isSaving = false;
          this.router.navigate(['/admin/blog']);
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Failed to update blog post.';
          this.isSaving = false;
        }
      });
    } else {
      this.blogService.addPost(formData).subscribe({
        next: () => {
          this.isSaving = false;
          this.router.navigate(['/admin/blog']);
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Failed to create blog post.';
          this.isSaving = false;
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/admin/blog']);
  }
}
