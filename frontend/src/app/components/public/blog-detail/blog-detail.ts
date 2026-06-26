import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Blog } from '../../../services/blog';
import { API_BASE_URL } from '../../../config';

@Component({
  selector: 'app-blog-detail',
  standalone: false,
  templateUrl: './blog-detail.html',
  styleUrl: './blog-detail.css',
})
export class BlogDetail implements OnInit {
  post: any;
  popularPosts: any[] = [];
  relatedPosts: any[] = [];
  comments: any[] = [];

  // Likes
  hasLiked = false;
  
  // Comment Form Model
  commentModel = {
    name: '',
    email: '',
    website: '',
    comment: '',
    parentCommentId: null as number | null
  };

  // Reply state
  activeReplyId: number | null = null;
  replyModel = {
    name: '',
    email: '',
    website: '',
    comment: ''
  };

  isLoading = true;
  commentSuccessMessage = '';
  commentErrorMessage = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly blogService: Blog
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.loadPost(slug);
      }
    });
  }

  loadPost(slug: string): void {
    this.isLoading = true;
    this.blogService.getPostBySlug(slug).subscribe({
      next: (post) => {
        this.post = post;
        this.isLoading = false;
        
        // Fetch Comments
        this.loadComments(post.postId);

        // Fetch Popular Posts
        this.blogService.getPopularPosts(5).subscribe({
          next: (pops) => this.popularPosts = pops
        });

        // Fetch Related Posts
        if (post.categoryId) {
          this.blogService.getRelatedPosts(post.postId, post.categoryId, 3).subscribe({
            next: (related) => this.relatedPosts = related
          });
        }
      },
      error: (err) => {
        console.error('Error fetching blog post', err);
        this.router.navigate(['/blog']);
      }
    });
  }

  loadComments(postId: number): void {
    this.blogService.getComments(postId).subscribe({
      next: (coms) => this.comments = coms,
      error: (err) => console.error('Error fetching comments', err)
    });
  }

  onLike(): void {
    if (this.hasLiked) return;
    
    const visitorId = parseInt(localStorage.getItem('visitor_id') || '0');
    this.blogService.toggleLike(this.post.postId, visitorId).subscribe({
      next: (res) => {
        if (res.liked) {
          this.post.likeCount++;
          this.hasLiked = true;
        }
      },
      error: (err) => console.error('Error toggling like', err)
    });
  }

  onSubmitComment(): void {
    this.commentSuccessMessage = '';
    this.commentErrorMessage = '';

    const payload = {
      name: this.commentModel.name,
      email: this.commentModel.email,
      website: this.commentModel.website,
      comment: this.commentModel.comment,
      parentCommentId: this.commentModel.parentCommentId,
      visitorId: parseInt(localStorage.getItem('visitor_id') || '0')
    };

    this.blogService.addComment(this.post.postId, payload).subscribe({
      next: (res) => {
        this.commentSuccessMessage = res.message || 'Comment submitted for approval!';
        // Reset comment form
        this.commentModel.name = '';
        this.commentModel.email = '';
        this.commentModel.website = '';
        this.commentModel.comment = '';
        this.commentModel.parentCommentId = null;
      },
      error: (err) => {
        this.commentErrorMessage = 'Failed to submit comment. Please try again.';
      }
    });
  }

  onReply(commentId: number): void {
    this.activeReplyId = commentId;
  }

  cancelReply(): void {
    this.activeReplyId = null;
    this.replyModel.name = '';
    this.replyModel.email = '';
    this.replyModel.website = '';
    this.replyModel.comment = '';
  }

  onSubmitReply(parentCommentId: number): void {
    this.commentSuccessMessage = '';
    this.commentErrorMessage = '';

    const payload = {
      name: this.replyModel.name,
      email: this.replyModel.email,
      website: this.replyModel.website,
      comment: this.replyModel.comment,
      parentCommentId: parentCommentId,
      visitorId: parseInt(localStorage.getItem('visitor_id') || '0')
    };

    this.blogService.addComment(this.post.postId, payload).subscribe({
      next: (res) => {
        this.commentSuccessMessage = res.message || 'Reply submitted for approval!';
        this.cancelReply();
      },
      error: (err) => {
        this.commentErrorMessage = 'Failed to submit reply. Please try again.';
      }
    });
  }

  assetUrl(path?: string): string {
    if (!path) return 'assets/img/blog-default.jpg';
    return path.startsWith('http') ? path : `${API_BASE_URL}${path}`;
  }
}

