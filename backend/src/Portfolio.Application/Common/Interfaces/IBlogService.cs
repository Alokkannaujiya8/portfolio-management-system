using Portfolio.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Application.Common.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogPost>> GetPublishedBlogPostsAsync(int? categoryId = null, string? tag = null, string? search = null, int page = 1, int pageSize = 6);
        Task<int> GetTotalBlogPostsCountAsync(int? categoryId = null, string? tag = null, string? search = null);
        Task<BlogPost?> GetBlogPostBySlugAsync(string slug);
        Task<BlogPost?> GetBlogPostByIdAsync(int id);
        
        Task<List<BlogCategory>> GetAllBlogCategoriesAsync();
        Task<int> AddBlogCategoryAsync(BlogCategory category);
        Task UpdateBlogCategoryAsync(BlogCategory category);
        Task DeleteBlogCategoryAsync(int id);

        Task<List<BlogPost>> GetPopularBlogPostsAsync(int count);
        Task<List<BlogPost>> GetRelatedBlogPostsAsync(int postId, int categoryId, int count);
        
        Task<List<BlogComment>> GetBlogCommentsAsync(int postId);
        Task<int> AddBlogCommentAsync(BlogComment comment);
        Task ApproveCommentAsync(int commentId);
        Task DeleteBlogCommentAsync(int commentId);
        Task MarkCommentAsSpamAsync(int commentId);

        Task<bool> ToggleLikeAsync(int postId, int visitorId, string ipAddress);
        Task<bool> HasUserLikedAsync(int postId, int visitorId);

        // Media items
        Task<int> AddBlogImageAsync(BlogImage image);
        Task DeleteBlogImageAsync(int imageId);
        Task<int> AddBlogVideoAsync(BlogVideo video);
        Task DeleteBlogVideoAsync(int videoId);

        // Admin CRUD
        Task<List<BlogPost>> GetAllBlogPostsAsync();
        Task<int> AddBlogPostAsync(BlogPost post);
        Task UpdateBlogPostAsync(BlogPost post);
        Task DeleteBlogPostAsync(int id);
    }
}
