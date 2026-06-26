using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Domain.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Portfolio.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _env;

        public BlogController(IBlogService blogService, IWebHostEnvironment env)
        {
            _blogService = blogService;
            _env = env;
        }

        // ==================== PUBLIC ENDPOINTS ====================

        [HttpGet("posts")]
        public async Task<IActionResult> GetPublishedPosts(
            [FromQuery] int? categoryId = null,
            [FromQuery] string? tag = null,
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6)
        {
            var posts = await _blogService.GetPublishedBlogPostsAsync(categoryId, tag, search, page, pageSize);
            var totalCount = await _blogService.GetTotalBlogPostsCountAsync(categoryId, tag, search);

            return Ok(new
            {
                posts = posts,
                totalCount = totalCount,
                page = page,
                pageSize = pageSize
            });
        }

        [HttpGet("posts/{slug}")]
        public async Task<IActionResult> GetPostBySlug(string slug)
        {
            var post = await _blogService.GetBlogPostBySlugAsync(slug);
            if (post == null)
            {
                return NotFound("Blog post not found");
            }
            return Ok(post);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _blogService.GetAllBlogCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularPosts([FromQuery] int count = 5)
        {
            var posts = await _blogService.GetPopularBlogPostsAsync(count);
            return Ok(posts);
        }

        [HttpGet("posts/{id}/related")]
        public async Task<IActionResult> GetRelatedPosts(int id, [FromQuery] int categoryId, [FromQuery] int count = 3)
        {
            var posts = await _blogService.GetRelatedBlogPostsAsync(id, categoryId, count);
            return Ok(posts);
        }

        [HttpPost("posts/{id}/like")]
        public async Task<IActionResult> ToggleLike(int id, [FromQuery] int visitorId)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "::1";
            var liked = await _blogService.ToggleLikeAsync(id, visitorId, ipAddress);
            return Ok(new { liked = liked });
        }

        [HttpGet("posts/{id}/comments")]
        public async Task<IActionResult> GetComments(int id)
        {
            var comments = await _blogService.GetBlogCommentsAsync(id);
            return Ok(comments);
        }

        [HttpPost("posts/{id}/comment")]
        public async Task<IActionResult> AddComment(int id, [FromBody] BlogComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            comment.PostId = id;
            comment.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var commentId = await _blogService.AddBlogCommentAsync(comment);
            return Ok(new { commentId = commentId, message = "Comment submitted successfully and is awaiting moderation." });
        }

        // ==================== ADMIN ENDPOINTS ====================

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/posts")]
        public async Task<IActionResult> AdminGetPosts()
        {
            var posts = await _blogService.GetAllBlogPostsAsync();
            return Ok(posts);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/posts/{id}")]
        public async Task<IActionResult> AdminGetPostById(int id)
        {
            var post = await _blogService.GetBlogPostByIdAsync(id);
            if (post == null)
            {
                return NotFound("Post not found");
            }
            return Ok(post);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin/posts")]
        public async Task<IActionResult> AddPost([FromForm] BlogPostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = new BlogPost
            {
                Title = request.Title,
                Slug = request.Slug,
                Excerpt = request.Excerpt,
                Content = request.Content,
                CategoryId = request.CategoryId,
                Tags = request.Tags,
                MetaTitle = request.MetaTitle,
                MetaDescription = request.MetaDescription,
                MetaKeywords = request.MetaKeywords,
                IsFeatured = request.IsFeatured,
                IsPublished = request.IsPublished,
                IsActive = true
            };

            if (request.FeaturedImageFile != null && request.FeaturedImageFile.Length > 0)
            {
                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsFolder = Path.Combine(webRootPath, "uploads", "blog");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.FeaturedImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.FeaturedImageFile.CopyToAsync(stream);
                }

                post.FeaturedImage = "/uploads/blog/" + fileName;
            }

            var postId = await _blogService.AddBlogPostAsync(post);
            return Ok(new { postId = postId, message = "Post created successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/posts")]
        public async Task<IActionResult> UpdatePost([FromForm] BlogPostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = new BlogPost
            {
                PostId = request.PostId,
                Title = request.Title,
                Slug = request.Slug,
                Excerpt = request.Excerpt,
                Content = request.Content,
                CategoryId = request.CategoryId,
                Tags = request.Tags,
                MetaTitle = request.MetaTitle,
                MetaDescription = request.MetaDescription,
                MetaKeywords = request.MetaKeywords,
                IsFeatured = request.IsFeatured,
                IsPublished = request.IsPublished,
                IsActive = true
            };

            if (request.FeaturedImageFile != null && request.FeaturedImageFile.Length > 0)
            {
                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsFolder = Path.Combine(webRootPath, "uploads", "blog");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.FeaturedImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.FeaturedImageFile.CopyToAsync(stream);
                }

                post.FeaturedImage = "/uploads/blog/" + fileName;
            }

            await _blogService.UpdateBlogPostAsync(post);
            return Ok(new { message = "Post updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/posts/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _blogService.DeleteBlogPostAsync(id);
            return Ok(new { message = "Post deleted successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin/categories")]
        public async Task<IActionResult> AddCategory([FromBody] BlogCategory category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryId = await _blogService.AddBlogCategoryAsync(category);
            return Ok(new { categoryId = categoryId, message = "Category added successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/categories")]
        public async Task<IActionResult> UpdateCategory([FromBody] BlogCategory category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _blogService.UpdateBlogCategoryAsync(category);
            return Ok(new { message = "Category updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _blogService.DeleteBlogCategoryAsync(id);
            return Ok(new { message = "Category deleted successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/comments/{id}/approve")]
        public async Task<IActionResult> ApproveComment(int id)
        {
            await _blogService.ApproveCommentAsync(id);
            return Ok(new { message = "Comment approved successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/comments/{id}/spam")]
        public async Task<IActionResult> MarkCommentSpam(int id)
        {
            await _blogService.MarkCommentAsSpamAsync(id);
            return Ok(new { message = "Comment marked as spam" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _blogService.DeleteBlogCommentAsync(id);
            return Ok(new { message = "Comment deleted successfully" });
        }
    }

    public class BlogPostRequest
    {
        public int PostId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Excerpt { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Tags { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsPublished { get; set; }
        public IFormFile? FeaturedImageFile { get; set; }
    }
}
