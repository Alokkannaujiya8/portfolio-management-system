using Microsoft.AspNetCore.Mvc;
using Portfolio_Management_System.DAL;
using Portfolio_Management_System.Models;

namespace Portfolio_Management_System.Controllers
{
    public class AdminGalleryController : Controller
    {
        private readonly ILogger<AdminGalleryController> _logger;
        private readonly DbHelper _dbHelper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminGalleryController(ILogger<AdminGalleryController> logger, DbHelper dbHelper, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _dbHelper = dbHelper;
            _hostEnvironment = hostEnvironment;
        }

        // GET: AdminGallery
        public async Task<IActionResult> Index(string? type)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            var items = await _dbHelper.GetAllGalleryItemsAsync(type);
            return View(items);
        }

        // GET: AdminGallery/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            return View(new GalleryModel());
        }

        // POST: AdminGallery/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GalleryModel model, IFormFile? imageFile, IFormFile? videoFile, IFormFile? documentFile)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            try
            {
                string uploadsFolder = "";
                string fileName = "";

                // Handle different media types
                switch (model.MediaType)
                {
                    case "image":
                        if (imageFile != null && imageFile.Length > 0)
                        {
                            uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "gallery", "images");
                            Directory.CreateDirectory(uploadsFolder);

                            fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }

                            model.MediaPath = "/uploads/gallery/images/" + fileName;

                            // Create thumbnail
                            var thumbnailPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "gallery", "thumbnails", fileName);
                            Directory.CreateDirectory(Path.GetDirectoryName(thumbnailPath));
                            // You can add thumbnail generation logic here
                            model.ThumbnailPath = "/uploads/gallery/thumbnails/" + fileName;
                        }
                        break;

                    case "video":
                        if (videoFile != null && videoFile.Length > 0)
                        {
                            uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "gallery", "videos");
                            Directory.CreateDirectory(uploadsFolder);

                            fileName = Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await videoFile.CopyToAsync(stream);
                            }

                            model.MediaPath = "/uploads/gallery/videos/" + fileName;

                            // Create thumbnail from video (optional)
                            model.ThumbnailPath = "/images/video-thumbnail.jpg";
                        }
                        else if (!string.IsNullOrEmpty(model.VideoEmbedCode))
                        {
                            model.MediaPath = model.VideoEmbedCode;
                        }
                        break;

                    case "document":
                        if (documentFile != null && documentFile.Length > 0)
                        {
                            uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "gallery", "documents");
                            Directory.CreateDirectory(uploadsFolder);

                            fileName = Guid.NewGuid().ToString() + Path.GetExtension(documentFile.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await documentFile.CopyToAsync(stream);
                            }

                            model.MediaPath = "/uploads/gallery/documents/" + fileName;
                            model.ThumbnailPath = "/images/document-thumbnail.jpg";
                        }
                        break;
                }

                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                model.IsActive = true;
                model.ViewCount = 0;
                model.DownloadCount = 0;

                await _dbHelper.AddGalleryItemAsync(model);
                TempData["Success"] = "Gallery item added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating gallery item");
                ModelState.AddModelError("", "Error creating item: " + ex.Message);
                return View(model);
            }
        }

        // GET: AdminGallery/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            var item = await _dbHelper.GetGalleryItemByIdAsync(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // POST: AdminGallery/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GalleryModel model, IFormFile? imageFile, IFormFile? videoFile, IFormFile? documentFile)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login", "Admin");

            try
            {
                var existingItem = await _dbHelper.GetGalleryItemByIdAsync(model.GalleryId);
                if (existingItem == null)
                    return NotFound();

                string uploadsFolder = "";
                string fileName = "";

                // Handle different media types
                switch (model.MediaType)
                {
                    case "image":
                        if (imageFile != null && imageFile.Length > 0)
                        {
                            uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "gallery", "images");
                            Directory.CreateDirectory(uploadsFolder);

                            fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }

                            model.MediaPath = "/uploads/gallery/images/" + fileName;

                            // Delete old file
                            if (!string.IsNullOrEmpty(existingItem.MediaPath))
                            {
                                var oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, existingItem.MediaPath.TrimStart('/'));
                                if (System.IO.File.Exists(oldFilePath))
                                    System.IO.File.Delete(oldFilePath);
                            }
                        }
                        else
                        {
                            model.MediaPath = existingItem.MediaPath;
                        }
                        break;

                    case "video":
                        if (videoFile != null && videoFile.Length > 0)
                        {
                            uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "gallery", "videos");
                            Directory.CreateDirectory(uploadsFolder);

                            fileName = Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await videoFile.CopyToAsync(stream);
                            }

                            model.MediaPath = "/uploads/gallery/videos/" + fileName;

                            // Delete old file
                            if (!string.IsNullOrEmpty(existingItem.MediaPath) && !existingItem.MediaPath.StartsWith("http"))
                            {
                                var oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, existingItem.MediaPath.TrimStart('/'));
                                if (System.IO.File.Exists(oldFilePath))
                                    System.IO.File.Delete(oldFilePath);
                            }
                        }
                        else if (!string.IsNullOrEmpty(model.VideoEmbedCode))
                        {
                            model.MediaPath = model.VideoEmbedCode;
                        }
                        else
                        {
                            model.MediaPath = existingItem.MediaPath;
                        }
                        break;

                    case "document":
                        if (documentFile != null && documentFile.Length > 0)
                        {
                            uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "gallery", "documents");
                            Directory.CreateDirectory(uploadsFolder);

                            fileName = Guid.NewGuid().ToString() + Path.GetExtension(documentFile.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await documentFile.CopyToAsync(stream);
                            }

                            model.MediaPath = "/uploads/gallery/documents/" + fileName;

                            // Delete old file
                            if (!string.IsNullOrEmpty(existingItem.MediaPath))
                            {
                                var oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, existingItem.MediaPath.TrimStart('/'));
                                if (System.IO.File.Exists(oldFilePath))
                                    System.IO.File.Delete(oldFilePath);
                            }
                        }
                        else
                        {
                            model.MediaPath = existingItem.MediaPath;
                        }
                        break;
                }

                model.UpdatedDate = DateTime.Now;
                model.CreatedDate = existingItem.CreatedDate;
                model.ViewCount = existingItem.ViewCount;
                model.DownloadCount = existingItem.DownloadCount;
                model.IsActive = existingItem.IsActive;

                await _dbHelper.UpdateGalleryItemAsync(model);
                TempData["Success"] = "Gallery item updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating gallery item");
                ModelState.AddModelError("", "Error updating item: " + ex.Message);
                return View(model);
            }
        }

        // POST: AdminGallery/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                // Get item to delete files
                var item = await _dbHelper.GetGalleryItemByIdAsync(id);
                if (item != null && !string.IsNullOrEmpty(item.MediaPath) && !item.MediaPath.StartsWith("http"))
                {
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, item.MediaPath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    if (!string.IsNullOrEmpty(item.ThumbnailPath))
                    {
                        var thumbPath = Path.Combine(_hostEnvironment.WebRootPath, item.ThumbnailPath.TrimStart('/'));
                        if (System.IO.File.Exists(thumbPath))
                            System.IO.File.Delete(thumbPath);
                    }
                }

                await _dbHelper.DeleteGalleryItemAsync(id);
                return Ok(new { success = true, message = "Item deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting gallery item");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}