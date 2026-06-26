using Microsoft.AspNetCore.Mvc;
using Portfolio_Management_System.DAL;
using Portfolio_Management_System.Models;

namespace Portfolio_Management_System.Controllers
{
    public class AdminBlogController : Controller
    {
        private readonly ILogger<AdminBlogController> _logger;
        private readonly DbHelper _dbHelper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminBlogController(ILogger<AdminBlogController> logger, DbHelper dbHelper, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _dbHelper = dbHelper;
            _hostEnvironment = hostEnvironment;
        }

        // GET: AdminBlog
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            var posts = await _dbHelper.GetAllBlogPostsAsync();
            return View(posts);
        }

        // GET: AdminBlog/Create
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            ViewBag.Categories = await _dbHelper.GetAllBlogCategoriesAsync();
            return View(new BlogPostModel());
        }

        // POST: AdminBlog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostModel model, IFormFile? featuredImage, List<IFormFile>? images, IFormFile? videoFile)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            try
            {
                // Generate slug if empty
                if (string.IsNullOrEmpty(model.Slug))
                {
                    model.Slug = BlogController.CreateSlug(model.Title);
                }

                // Handle featured image
                if (featuredImage != null && featuredImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "blog", "featured");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(featuredImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await featuredImage.CopyToAsync(stream);
                    }

                    model.FeaturedImage = "/uploads/blog/featured/" + fileName;
                }

                // Set dates
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                model.IsActive = true;

                var postId = await _dbHelper.AddBlogPostAsync(model);

                // Handle additional images
                if (images != null && images.Any())
                {
                    int displayOrder = 0;
                    foreach (var image in images)
                    {
                        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "blog", "images");
                        Directory.CreateDirectory(uploadsFolder);

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        var imagePath = "/uploads/blog/images/" + fileName;
                        await _dbHelper.AddBlogImageAsync(postId, imagePath, null, "", "", displayOrder, false);
                        displayOrder++;
                    }
                }

                // Handle video
                if (videoFile != null && videoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "blog", "videos");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await videoFile.CopyToAsync(stream);
                    }

                    var videoPath = "/uploads/blog/videos/" + fileName;
                    await _dbHelper.AddBlogVideoAsync(postId, model.Title, videoPath, "MP4", null, 0);
                }

                TempData["Success"] = "Blog post created successfully!";
                return RedirectToAction("Edit", new { id = postId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blog post");
                ModelState.AddModelError("", "Error creating post: " + ex.Message);

                ViewBag.Categories = await _dbHelper.GetAllBlogCategoriesAsync();
                return View(model);
            }
        }

        // GET: AdminBlog/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            var post = await _dbHelper.GetBlogPostByIdAsync(id);
            if (post == null)
                return NotFound();

            ViewBag.Categories = await _dbHelper.GetAllBlogCategoriesAsync();
            return View(post);
        }

        // POST: AdminBlog/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogPostModel model, IFormFile? newFeaturedImage)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            try
            {
                // Generate slug if empty
                if (string.IsNullOrEmpty(model.Slug))
                {
                    model.Slug = BlogController.CreateSlug(model.Title);
                }

                // Handle new featured image
                if (newFeaturedImage != null && newFeaturedImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "blog", "featured");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newFeaturedImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await newFeaturedImage.CopyToAsync(stream);
                    }

                    model.FeaturedImage = "/uploads/blog/featured/" + fileName;
                }

                model.UpdatedDate = DateTime.Now;
                await _dbHelper.UpdateBlogPostAsync(model);

                TempData["Success"] = "Blog post updated successfully!";
                return RedirectToAction("Edit", new { id = model.PostId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog post");
                ModelState.AddModelError("", "Error updating post: " + ex.Message);

                ViewBag.Categories = await _dbHelper.GetAllBlogCategoriesAsync();
                return View(model);
            }
        }

        // POST: AdminBlog/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                await _dbHelper.DeleteBlogPostAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blog post");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: AdminBlog/DeleteImage
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                await _dbHelper.DeleteBlogImageAsync(imageId);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image");
                return StatusCode(500, new { success = false });
            }
        }

        // GET: AdminBlog/Categories
        public async Task<IActionResult> Categories()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            var categories = await _dbHelper.GetAllBlogCategoriesAsync();
            return View(categories);
        }

        // POST: AdminBlog/AddCategory
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] BlogCategoryModel category)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false, message = "Unauthorized" });

            try
            {
                // Generate slug if empty
                if (string.IsNullOrEmpty(category.Slug))
                {
                    category.Slug = BlogController.CreateSlug(category.CategoryName);
                }

                category.CreatedDate = DateTime.Now;
                category.UpdatedDate = DateTime.Now;
                category.IsActive = true;

                var id = await _dbHelper.AddBlogCategoryAsync(category);
                return Ok(new { success = true, id = id, message = "Category added successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add category error");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: AdminBlog/UpdateCategory
        [HttpPost]
        public async Task<IActionResult> UpdateCategory([FromBody] BlogCategoryModel category)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false, message = "Unauthorized" });

            try
            {
                // Generate slug if empty
                if (string.IsNullOrEmpty(category.Slug))
                {
                    category.Slug = BlogController.CreateSlug(category.CategoryName);
                }

                category.UpdatedDate = DateTime.Now;
                await _dbHelper.UpdateBlogCategoryAsync(category);
                return Ok(new { success = true, message = "Category updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update category error");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: AdminBlog/DeleteCategory
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                // Check if category has posts
                var postCount = await _dbHelper.GetBlogPostCountByCategoryAsync(id);
                if (postCount > 0)
                {
                    return Ok(new { success = false, message = "Cannot delete category with existing posts." });
                }

                await _dbHelper.DeleteBlogCategoryAsync(id);
                return Ok(new { success = true, message = "Category deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete category error");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: AdminBlog/Comments
        public async Task<IActionResult> Comments()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            var comments = await _dbHelper.GetAllCommentsAsync();
            return View(comments);
        }
        public async Task<IActionResult> GetAllComments()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized();

            var comments = await _dbHelper.GetAllCommentsAsync();
            return Json(comments);
        }

        // POST: AdminBlog/ApproveComment
        [HttpPost]
        public async Task<IActionResult> ApproveComment(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                await _dbHelper.ApproveCommentAsync(id);
                return Ok(new { success = true, message = "Comment approved successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving comment");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: AdminBlog/DeleteComment
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                await _dbHelper.DeleteCommentAsync(id);
                return Ok(new { success = true, message = "Comment deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: AdminBlog/AddComment (Admin reply)
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] BlogCommentModel comment)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                comment.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                comment.CreatedDate = DateTime.Now;
                comment.IsActive = true;
                comment.IsApproved = true; // Admin replies are auto-approved

                var id = await _dbHelper.AddBlogCommentAsync(comment);
                return Ok(new { success = true, id = id, message = "Reply added successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}