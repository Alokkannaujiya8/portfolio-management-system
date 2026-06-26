using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Portfolio_Management_System.DAL;
using Portfolio_Management_System.Models;
using System.Text.RegularExpressions;

namespace Portfolio_Management_System.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly DbHelper _dbHelper;
        private readonly string _connectionString;
        public BlogController(ILogger<BlogController> logger, DbHelper dbHelper)
        {
            _logger = logger;
            _dbHelper = dbHelper;
        }

        // GET: Blog/Index
        public async Task<IActionResult> Index(int? categoryId, string? tag, string? search, int page = 1)
        {
            try
            {
                var posts = await _dbHelper.GetPublishedBlogPostsAsync(categoryId, tag, search, page, 6);
                var categories = await _dbHelper.GetAllBlogCategoriesAsync();
                var popularPosts = await _dbHelper.GetPopularBlogPostsAsync(5);
                var totalPosts = await _dbHelper.GetTotalBlogPostsCountAsync(categoryId, tag, search);
                var totalPages = (int)Math.Ceiling(totalPosts / 6.0);

                foreach (var cat in categories)
                {
                    cat.PostCount = await _dbHelper.GetBlogPostCountByCategoryAsync(cat.CategoryId);
                }

                ViewBag.Categories = categories;
                ViewBag.PopularPosts = popularPosts;
                ViewBag.CurrentCategory = categoryId.HasValue ? categories.FirstOrDefault(c => c.CategoryId == categoryId) : null;
                ViewBag.CurrentTag = tag;
                ViewBag.SearchTerm = search;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;

                return View(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading blog index");
                return View(new List<BlogPostModel>());
            }
        }


        // GET: Blog/Test
        public IActionResult Test()
        {
            var testPost = new BlogPostModel
            {
                PostId = 999,
                Title = "Test Blog Post",
                Slug = "test-post",
                Content = "<h1>Welcome to Test Post</h1><p>This is a test post.</p>",
                FeaturedImage = "/images/default-avatar.jpg",
                CreatedDate = DateTime.Now,
                ViewCount = 100,
                LikeCount = 50
            };

            var viewModel = new BlogPostViewModel
            {
                Post = testPost,
                Categories = new List<BlogCategoryModel>(),
                RelatedPosts = new List<BlogPostModel>(),
                PopularPosts = new List<BlogPostModel>(),
                NewComment = new BlogCommentModel(),
                HasUserLiked = false
            };

            return View("Post", viewModel);
        }

        // ========== LIKE SYSTEM - FIXED VERSION ==========

        // POST: Blog/ToggleLike
        [HttpPost]
        public async Task<IActionResult> ToggleLike(int postId)
        {
            try
            {
                Console.WriteLine($"========== TOGGLE LIKE CALLED ==========");
                Console.WriteLine($"PostId: {postId}");

                // Visitor ID बनाओ या लो
                int visitorId = await GetOrCreateVisitorId();
                Console.WriteLine($"Visitor ID: {visitorId}");

                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                // Like toggle करो
                var result = await _dbHelper.ToggleBlogLikeAsync(postId, visitorId, ipAddress);
                var post = await _dbHelper.GetBlogPostByIdAsync(postId);

                return Json(new
                {
                    success = true,
                    action = result,
                    likeCount = post?.LikeCount ?? 0
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                _logger.LogError(ex, "Error toggling like");
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<int> GetOrCreateVisitorId()
        {
            // Session से check करो
            var visitorIdStr = HttpContext.Session.GetString("VisitorId");

            if (!string.IsNullOrEmpty(visitorIdStr) && int.TryParse(visitorIdStr, out int existingId))
            {
                // क्या ये visitor database में है?
                var visitor = await _dbHelper.GetVisitorByIdAsync(existingId);
                if (visitor != null)
                {
                    return existingId;
                }
            }

            // नया visitor बनाओ
            var newVisitor = new VisitorTrackingModel
            {
                SessionId = HttpContext.Session.Id,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = Request.Headers.UserAgent.ToString(),
                FirstVisit = DateTime.Now,
                LastVisit = DateTime.Now,
                VisitCount = 1,
                IsActive = true
            };

            var newVisitorId = await _dbHelper.CreateVisitorAsync(newVisitor);

            // Session में save करो
            HttpContext.Session.SetString("VisitorId", newVisitorId.ToString());

            return newVisitorId;
        }

        // POST: Blog/AddComment
        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string Name, string Email, string Comment)
        {
            try
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Comment))
                {
                    return Json(new { success = false, message = "All fields are required" });
                }

                var comment = new BlogCommentModel
                {
                    PostId = postId,
                    Name = Name,
                    Email = Email,
                    Comment = Comment,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    CreatedDate = DateTime.Now,
                    IsApproved = false,
                    IsActive = true
                };

                await _dbHelper.AddBlogCommentAsync(comment);
                return Json(new { success = true, message = "Comment submitted for approval!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                return Json(new { success = false, message = ex.Message });
            }
        }
        public async Task<IActionResult> Post(string slug)
        {
            try
            {
                Console.WriteLine("========== BLOG POST METHOD CALLED ==========");
                Console.WriteLine($"Received slug: '{slug}'");
                Console.WriteLine($"Slug is null or empty: {string.IsNullOrEmpty(slug)}");

                if (string.IsNullOrEmpty(slug))
                {
                  
                    var path = Request.Path.Value ?? "";
                    Console.WriteLine($"Full path: {path}");

                    if (path.Contains("/Post/"))
                    {
                        var parts = path.Split("/Post/");
                        if (parts.Length > 1)
                        {
                            slug = parts[1].Trim('/');
                            Console.WriteLine($"Extracted slug from URL: '{slug}'");
                        }
                    }

                    if (string.IsNullOrEmpty(slug))
                    {
                        Console.WriteLine("❌ Slug is empty - returning NotFound");
                        return NotFound();
                    }
                }

                Console.WriteLine($"Calling _dbHelper.GetBlogPostBySlugAsync('{slug}')");
                var post = await _dbHelper.GetBlogPostBySlugAsync(slug);

                if (post == null)
                {
                    Console.WriteLine($"❌ Post not found for slug: '{slug}'");
                    return NotFound();
                }

                Console.WriteLine($"✅ Post found: ID={post.PostId}, Title='{post.Title}'");
                Console.WriteLine($"Post IsPublished: {post.IsPublished}, IsActive: {post.IsActive}");

                var viewModel = new BlogPostViewModel
                {
                    Post = post,
                    Categories = new List<BlogCategoryModel>(),
                    RelatedPosts = new List<BlogPostModel>(),
                    PopularPosts = new List<BlogPostModel>(),
                    NewComment = new BlogCommentModel(),
                    HasUserLiked = false
                };

                Console.WriteLine("✅ Returning View");
                return View("Post", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EXCEPTION: {ex.Message}");
                _logger.LogError(ex, "Error loading blog post: {Slug}", slug);
                return NotFound();
            }
        }
        public static string CreateSlug(string title)
        {
            if (string.IsNullOrEmpty(title)) return "";
            var slug = title.ToLower().Trim();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"-+", "-");
            return slug;
        }
        public async Task<string> ToggleBlogLikeAsync(int postId, int visitorId, string? ipAddress)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            // Check if already liked
            var checkSql = "SELECT COUNT(*) FROM BlogLikes WHERE PostId = @PostId AND VisitorId = @VisitorId";
            using (var checkCmd = new SqlCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("@PostId", postId);
                checkCmd.Parameters.AddWithValue("@VisitorId", visitorId);

                var exists = (int)await checkCmd.ExecuteScalarAsync() > 0;

                if (exists)
                {
                    // Unlike
                    var deleteSql = "DELETE FROM BlogLikes WHERE PostId = @PostId AND VisitorId = @VisitorId";
                    using (var deleteCmd = new SqlCommand(deleteSql, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@PostId", postId);
                        deleteCmd.Parameters.AddWithValue("@VisitorId", visitorId);
                        await deleteCmd.ExecuteNonQueryAsync();
                    }

                    var updateSql = "UPDATE BlogPosts SET LikeCount = LikeCount - 1 WHERE PostId = @PostId";
                    using (var updateCmd = new SqlCommand(updateSql, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@PostId", postId);
                        await updateCmd.ExecuteNonQueryAsync();
                    }

                    return "unliked";
                }
                else
                {
                    // Like
                    var insertSql = "INSERT INTO BlogLikes (PostId, VisitorId, IPAddress, CreatedDate) VALUES (@PostId, @VisitorId, @IPAddress, GETDATE())";
                    using (var insertCmd = new SqlCommand(insertSql, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@PostId", postId);
                        insertCmd.Parameters.AddWithValue("@VisitorId", visitorId);
                        insertCmd.Parameters.AddWithValue("@IPAddress", ipAddress ?? "");
                        await insertCmd.ExecuteNonQueryAsync();
                    }

                    var updateSql = "UPDATE BlogPosts SET LikeCount = LikeCount + 1 WHERE PostId = @PostId";
                    using (var updateCmd = new SqlCommand(updateSql, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@PostId", postId);
                        await updateCmd.ExecuteNonQueryAsync();
                    }

                    return "liked";
                }
            }
        }
    }
}