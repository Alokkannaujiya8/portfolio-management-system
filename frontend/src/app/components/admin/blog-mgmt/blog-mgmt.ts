import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Blog } from '../../../services/blog';

@Component({
  selector: 'app-blog-mgmt',
  standalone: false,
  templateUrl: './blog-mgmt.html',
  styleUrl: './blog-mgmt.css',
})
export class BlogMgmt implements OnInit {
  posts: any[] = [];
  categories: any[] = [];
  comments: any[] = [];
  
  activeTab = 'posts'; // 'posts' | 'categories' | 'comments'
  isLoading = true;
  isSavingCategory = false;
  
  newCategoryName = '';
  successMessage = '';
  errorMessage = '';

  constructor(
    private readonly blogService: Blog,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.successMessage = '';
    this.errorMessage = '';

    if (this.activeTab === 'posts') {
      this.blogService.adminGetPosts().subscribe({
        next: (data) => {
          this.posts = data;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error fetching blog posts', err);
          this.errorMessage = 'Failed to load blog posts.';
          this.isLoading = false;
        }
      });
    } else if (this.activeTab === 'categories') {
      this.blogService.getCategories().subscribe({
        next: (data) => {
          this.categories = data;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error fetching categories', err);
          this.errorMessage = 'Failed to load categories.';
          this.isLoading = false;
        }
      });
    } else if (this.activeTab === 'comments') {
      this.blogService.adminGetComments().subscribe({
        next: (data) => {
          this.comments = data;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error fetching comments', err);
          this.errorMessage = 'Failed to load comments.';
          this.isLoading = false;
        }
      });
    }
  }

  switchTab(tab: string): void {
    this.activeTab = tab;
    this.loadData();
  }

  // ==================== POST MANAGEMENT ====================

  onAddPost(): void {
    this.router.navigate(['/admin/blog/new']);
  }

  onEditPost(id: number): void {
    this.router.navigate([`/admin/blog/edit/${id}`]);
  }

  onDeletePost(id: number): void {
    if (confirm('Are you sure you want to delete this blog post?')) {
      this.blogService.deletePost(id).subscribe({
        next: () => {
          this.successMessage = 'Post deleted successfully.';
          this.loadData();
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete post.';
          console.error(err);
        }
      });
    }
  }

  // ==================== CATEGORY MANAGEMENT ====================

  onAddCategory(): void {
    if (!this.newCategoryName.trim()) return;
    this.isSavingCategory = true;
    
    this.blogService.adminAddCategory({ categoryName: this.newCategoryName.trim() }).subscribe({
      next: () => {
        this.successMessage = 'Category added successfully!';
        this.newCategoryName = '';
        this.isSavingCategory = false;
        this.loadData();
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to add category.';
        this.isSavingCategory = false;
      }
    });
  }

  onDeleteCategory(id: number): void {
    if (confirm('Are you sure you want to delete this category? All posts in this category might be affected.')) {
      this.blogService.adminDeleteCategory(id).subscribe({
        next: () => {
          this.successMessage = 'Category deleted successfully.';
          this.loadData();
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Failed to delete category.';
          console.error(err);
        }
      });
    }
  }

  // ==================== COMMENTS MANAGEMENT ====================

  onApproveComment(id: number): void {
    this.blogService.adminApproveComment(id).subscribe({
      next: () => {
        this.successMessage = 'Comment approved successfully.';
        this.loadData();
      },
      error: (err) => {
        this.errorMessage = 'Failed to approve comment.';
        console.error(err);
      }
    });
  }

  onDeleteComment(id: number): void {
    if (confirm('Are you sure you want to delete this comment?')) {
      this.blogService.adminDeleteComment(id).subscribe({
        next: () => {
          this.successMessage = 'Comment deleted successfully.';
          this.loadData();
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete comment.';
          console.error(err);
        }
      });
    }
  }
}
