import { Component, OnInit } from '@angular/core';
import { Gallery as GalleryService } from '../../../services/gallery';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { API_BASE_URL } from '../../../config';

@Component({
  selector: 'app-gallery',
  standalone: false,
  templateUrl: './gallery.html',
  styleUrl: './gallery.css',
})
export class Gallery implements OnInit {
  items: any[] = [];
  categories: string[] = [];
  
  selectedType = ''; // '', 'Image', 'Video'
  selectedCategory = '';
  
  // Lightbox modal state
  selectedItem: any = null;
  safeVideoUrl: SafeResourceUrl | null = null;
  
  isLoading = true;

  constructor(
    private readonly galleryService: GalleryService,
    private readonly sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadItems();
  }

  loadCategories(): void {
    this.galleryService.getCategories().subscribe({
      next: (cats) => this.categories = cats,
      error: (err) => console.error('Error fetching categories', err)
    });
  }

  loadItems(): void {
    this.isLoading = true;
    this.galleryService.getGalleryItems(this.selectedType || undefined).subscribe({
      next: (res) => {
        this.items = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching gallery items', err);
        this.isLoading = false;
      }
    });
  }

  filterByType(type: string): void {
    this.selectedType = type;
    this.loadItems();
  }

  filterByCategory(cat: string): void {
    this.selectedCategory = cat;
  }

  getFilteredItems(): any[] {
    if (!this.selectedCategory) return this.items;
    return this.items.filter(item => item.category === this.selectedCategory);
  }

  openLightbox(item: any): void {
    this.selectedItem = item;
    
    // Log view count increment
    this.galleryService.incrementView(item.galleryId).subscribe({
      next: () => item.viewCount++
    });

    if (item.mediaType === 'Video' || item.videoEmbedCode) {
      const videoId = this.extractYoutubeId(item.videoEmbedCode || item.mediaPath);
      if (videoId) {
        this.safeVideoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(`https://www.youtube.com/embed/${videoId}`);
      } else {
        this.safeVideoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(item.mediaPath);
      }
    } else {
      this.safeVideoUrl = null;
    }
  }

  closeLightbox(): void {
    this.selectedItem = null;
    this.safeVideoUrl = null;
  }

  downloadItem(event: Event, item: any): void {
    event.stopPropagation();
    
    this.galleryService.incrementDownload(item.galleryId).subscribe({
      next: () => item.downloadCount++
    });
    
    // Download logic
    const link = document.createElement('a');
    link.href = this.assetUrl(item.mediaPath);
    link.target = '_blank';
    link.download = item.title || 'download';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  assetUrl(path?: string): string {
    if (!path) return '';
    return path.startsWith('http') ? path : `${API_BASE_URL}${path}`;
  }

  private extractYoutubeId(url?: string): string | null {
    if (!url) return null;
    const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
    const match = url.match(regExp);
    return (match && match[2].length === 11) ? match[2] : null;
  }
}

