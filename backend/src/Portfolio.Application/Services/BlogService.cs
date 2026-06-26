using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly IPortfolioDbContext _context;

        public BlogService(IPortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<List<BlogPost>> GetPublishedBlogPostsAsync(int? categoryId = null, string? tag = null, string? search = null, int page = 1, int pageSize = 6)
        {
            var query = _context.BlogPosts
                .Include(b => b.Category)
                .Where(b => b.IsActive && b.IsPublished == true);

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(tag))
            {
                // Tags are comma separated (e.g. "csharp, angular, api")
                query = query.Where(b => b.Tags != null && b.Tags.Contains(tag));
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => 
                    b.Title.Contains(search) || 
                    (b.Excerpt != null && b.Excerpt.Contains(search)) || 
                    (b.Content != null && b.Content.Contains(search))
                );
            }

            return await query
                .OrderByDescending(b => b.PublishedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalBlogPostsCountAsync(int? categoryId = null, string? tag = null, string? search = null)
        {
            var query = _context.BlogPosts.Where(b => b.IsActive && b.IsPublished == true);

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(b => b.Tags != null && b.Tags.Contains(tag));
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => 
                    b.Title.Contains(search) || 
                    (b.Excerpt != null && b.Excerpt.Contains(search)) || 
                    (b.Content != null && b.Content.Contains(search))
                );
            }

            return await query.CountAsync();
        }

        public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
        {
            var post = await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.Images.Where(i => i.IsActive == true))
                .Include(b => b.Videos.Where(v => v.IsActive == true))
                .Include(b => b.Comments.Where(c => c.IsActive == true && c.IsApproved == true))
                .FirstOrDefaultAsync(b => b.Slug == slug && b.IsActive);

            if (post != null)
            {
                // Increment views
                post.ViewCount = (post.ViewCount ?? 0) + 1;
                await _context.SaveChangesAsync();
            }

            return post;
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(int id)
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.Images.Where(i => i.IsActive == true))
                .Include(b => b.Videos.Where(v => v.IsActive == true))
                .FirstOrDefaultAsync(b => b.PostId == id && b.IsActive);
        }

        public async Task<List<BlogCategory>> GetAllBlogCategoriesAsync()
        {
            // Compute PostCount on the fly
            var categories = await _context.BlogCategories
                .Include(c => c.BlogPosts.Where(p => p.IsActive && p.IsPublished == true))
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            return categories;
        }

        public async Task<int> AddBlogCategoryAsync(BlogCategory category)
        {
            category.CreatedDate = DateTime.UtcNow;
            category.UpdatedDate = DateTime.UtcNow;
            _context.BlogCategories.Add(category);
            await _context.SaveChangesAsync();
            return category.CategoryId;
        }

        public async Task UpdateBlogCategoryAsync(BlogCategory category)
        {
            var existing = await _context.BlogCategories.FindAsync(category.CategoryId);
            if (existing != null)
            {
                existing.CategoryName = category.CategoryName;
                existing.Slug = category.Slug;
                existing.Description = category.Description;
                existing.Icon = category.Icon;
                existing.DisplayOrder = category.DisplayOrder;
                existing.IsActive = category.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBlogCategoryAsync(int id)
        {
            var category = await _context.BlogCategories.FindAsync(id);
            if (category != null)
            {
                category.IsActive = false;
                category.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BlogPost>> GetPopularBlogPostsAsync(int count)
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Where(b => b.IsActive && b.IsPublished == true)
                .OrderByDescending(b => b.ViewCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<BlogPost>> GetRelatedBlogPostsAsync(int postId, int categoryId, int count)
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Where(b => b.IsActive && b.IsPublished == true && b.CategoryId == categoryId && b.PostId != postId)
                .OrderByDescending(b => b.PublishedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<BlogComment>> GetBlogCommentsAsync(int postId)
        {
            var comments = await _context.BlogComments
                .Where(c => c.PostId == postId && c.IsActive == true && c.IsApproved == true)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            // Structure replies hierarchically
            var lookup = comments.ToLookup(c => c.ParentCommentId);
            var rootComments = comments.Where(c => c.ParentCommentId == null).ToList();
            
            foreach (var root in rootComments)
            {
                root.Replies = lookup[root.CommentId].OrderBy(r => r.CreatedDate).ToList();
            }

            return rootComments;
        }

        public async Task<int> AddBlogCommentAsync(BlogComment comment)
        {
            comment.CreatedDate = DateTime.UtcNow;
            comment.IsActive = true;
            comment.IsApproved = false; // Requires admin moderation
            comment.IsSpam = false;
            comment.LikeCount = 0;

            _context.BlogComments.Add(comment);
            await _context.SaveChangesAsync();

            // Increment comments count on post
            var post = await _context.BlogPosts.FindAsync(comment.PostId);
            if (post != null)
            {
                post.CommentCount = (post.CommentCount ?? 0) + 1;
                await _context.SaveChangesAsync();
            }

            return comment.CommentId;
        }

        public async Task ApproveCommentAsync(int commentId)
        {
            var comment = await _context.BlogComments.FindAsync(commentId);
            if (comment != null)
            {
                comment.IsApproved = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBlogCommentAsync(int commentId)
        {
            var comment = await _context.BlogComments.FindAsync(commentId);
            if (comment != null)
            {
                comment.IsActive = false;
                await _context.SaveChangesAsync();

                // Decrement comments count on post
                var post = await _context.BlogPosts.FindAsync(comment.PostId);
                if (post != null)
                {
                    post.CommentCount = Math.Max(0, (post.CommentCount ?? 0) - 1);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task MarkCommentAsSpamAsync(int commentId)
        {
            var comment = await _context.BlogComments.FindAsync(commentId);
            if (comment != null)
            {
                comment.IsSpam = true;
                comment.IsApproved = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ToggleLikeAsync(int postId, int visitorId, string ipAddress)
        {
            var existingLike = await _context.BlogLikes
                .FirstOrDefaultAsync(l => l.PostId == postId && (l.VisitorId == visitorId || l.IPAddress == ipAddress));

            var post = await _context.BlogPosts.FindAsync(postId);
            if (post == null) return false;

            if (existingLike != null)
            {
                // Unlike
                _context.BlogLikes.Remove(existingLike);
                post.LikeCount = Math.Max(0, (post.LikeCount ?? 0) - 1);
                await _context.SaveChangesAsync();
                return false;
            }
            else
            {
                // Like
                var newLike = new BlogLike
                {
                    PostId = postId,
                    VisitorId = visitorId > 0 ? visitorId : (int?)null,
                    IPAddress = ipAddress,
                    CreatedDate = DateTime.UtcNow
                };
                _context.BlogLikes.Add(newLike);
                post.LikeCount = (post.LikeCount ?? 0) + 1;
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> HasUserLikedAsync(int postId, int visitorId)
        {
            if (visitorId <= 0) return false;
            return await _context.BlogLikes.AnyAsync(l => l.PostId == postId && l.VisitorId == visitorId);
        }

        public async Task<int> AddBlogImageAsync(BlogImage image)
        {
            image.CreatedDate = DateTime.UtcNow;
            image.IsActive = true;
            _context.BlogImages.Add(image);
            await _context.SaveChangesAsync();
            return image.ImageId;
        }

        public async Task DeleteBlogImageAsync(int imageId)
        {
            var image = await _context.BlogImages.FindAsync(imageId);
            if (image != null)
            {
                image.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> AddBlogVideoAsync(BlogVideo video)
        {
            video.CreatedDate = DateTime.UtcNow;
            video.IsActive = true;
            _context.BlogVideos.Add(video);
            await _context.SaveChangesAsync();
            return video.VideoId;
        }

        public async Task DeleteBlogVideoAsync(int videoId)
        {
            var video = await _context.BlogVideos.FindAsync(videoId);
            if (video != null)
            {
                video.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BlogPost>> GetAllBlogPostsAsync()
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();
        }

        public async Task<int> AddBlogPostAsync(BlogPost post)
        {
            post.CreatedDate = DateTime.UtcNow;
            post.UpdatedDate = DateTime.UtcNow;
            post.ViewCount = 0;
            post.LikeCount = 0;
            post.CommentCount = 0;

            if (post.IsPublished == true)
            {
                post.PublishedDate = DateTime.UtcNow;
            }

            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
            return post.PostId;
        }

        public async Task UpdateBlogPostAsync(BlogPost post)
        {
            var existing = await _context.BlogPosts.FindAsync(post.PostId);
            if (existing != null)
            {
                existing.Title = post.Title;
                existing.Slug = post.Slug;
                existing.Excerpt = post.Excerpt;
                existing.Content = post.Content;
                existing.CategoryId = post.CategoryId;
                existing.Tags = post.Tags;
                existing.MetaTitle = post.MetaTitle;
                existing.MetaDescription = post.MetaDescription;
                existing.MetaKeywords = post.MetaKeywords;
                existing.IsFeatured = post.IsFeatured;
                existing.UpdatedDate = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(post.FeaturedImage))
                {
                    existing.FeaturedImage = post.FeaturedImage;
                }

                if (existing.IsPublished != true && post.IsPublished == true)
                {
                    existing.PublishedDate = DateTime.UtcNow;
                }
                existing.IsPublished = post.IsPublished;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBlogPostAsync(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post != null)
            {
                post.IsActive = false;
                post.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
