import { Component, OnInit } from '@angular/core';
import { Blog as BlogService } from '../../../services/blog';

@Component({
  selector: 'app-blog',
  standalone: false,
  templateUrl: './blog.html',
  styleUrl: './blog.css',
})
export class Blog implements OnInit {
  posts: any[] = [];
  categories: any[] = [];
  popularPosts: any[] = [];
  
  // Search & filter parameters
  categoryId?: number;
  selectedTag?: string;
  searchTerm = '';
  
  // Pagination
  currentPage = 1;
  pageSize = 6;
  totalCount = 0;
  totalPages = 0;
  
  isLoading = true;

  constructor(private readonly blogService: BlogService) {}

  ngOnInit(): void {
    this.loadInitialData();
  }

  loadInitialData(): void {
    this.blogService.getCategories().subscribe({
      next: (cats) => this.categories = cats,
      error: (err) => console.error('Error fetching categories', err)
    });

    this.blogService.getPopularPosts(5).subscribe({
      next: (pops) => this.popularPosts = pops,
      error: (err) => console.error('Error fetching popular posts', err)
    });

    this.loadPosts();
  }

  loadPosts(): void {
    this.isLoading = true;
    this.blogService.getPublishedPosts({
      categoryId: this.categoryId,
      tag: this.selectedTag,
      search: this.searchTerm,
      page: this.currentPage,
      pageSize: this.pageSize
    }).subscribe({
      next: (res) => {
        this.posts = res.posts;
        this.totalCount = res.totalCount;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching posts', err);
        this.isLoading = false;
      }
    });
  }

  filterByCategory(catId?: number): void {
    this.categoryId = catId;
    this.currentPage = 1;
    this.loadPosts();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadPosts();
  }

  filterByTag(tag: string): void {
    this.selectedTag = tag;
    this.currentPage = 1;
    this.loadPosts();
  }

  clearFilters(): void {
    this.categoryId = undefined;
    this.selectedTag = undefined;
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadPosts();
  }

  setPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadPosts();
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  assetUrl(path?: string): string {
    if (!path) return 'assets/img/blog-default.jpg';
    return path.startsWith('http') ? path : `http://localhost:5154${path}`;
  }

  getPagesArray(): number[] {
    const pages = [];
    for (let i = 1; i <= this.totalPages; i++) {
      pages.push(i);
    }
    return pages;
  }
}

