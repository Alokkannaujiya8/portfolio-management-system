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
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;
        private readonly IWebHostEnvironment _env;

        public GalleryController(IGalleryService galleryService, IWebHostEnvironment env)
        {
            _galleryService = galleryService;
            _env = env;
        }

        // ==================== PUBLIC ENDPOINTS ====================

        [HttpGet]
        public async Task<IActionResult> GetGalleryItems([FromQuery] string? type = null)
        {
            var items = await _galleryService.GetAllGalleryItemsAsync(type);
            return Ok(items);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _galleryService.GetGalleryCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _galleryService.GetGalleryItemByIdAsync(id);
            if (item == null) return NotFound("Gallery item not found");
            return Ok(item);
        }

        [HttpPut("{id}/view")]
        public async Task<IActionResult> IncrementViewCount(int id)
        {
            await _galleryService.IncrementGalleryViewCountAsync(id);
            return Ok(new { message = "View count incremented" });
        }

        [HttpPut("{id}/download")]
        public async Task<IActionResult> IncrementDownloadCount(int id)
        {
            await _galleryService.IncrementGalleryDownloadCountAsync(id);
            return Ok(new { message = "Download count incremented" });
        }

        // ==================== ADMIN ENDPOINTS ====================

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> AddItem([FromForm] GalleryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new Gallery
            {
                Title = request.Title,
                Description = request.Description,
                MediaType = request.MediaType,
                Category = request.Category,
                Tags = request.Tags,
                DisplayOrder = request.DisplayOrder,
                IsFeatured = request.IsFeatured,
                VideoEmbedCode = request.VideoEmbedCode,
                IsActive = true
            };

            var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Handle Image Upload
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var folder = Path.Combine(webRootPath, "uploads", "gallery", "images");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                item.MediaPath = "/uploads/gallery/images/" + fileName;
                item.ThumbnailPath = "/uploads/gallery/images/" + fileName;
            }

            // Handle Document Upload (if applicable)
            if (request.DocumentFile != null && request.DocumentFile.Length > 0)
            {
                var folder = Path.Combine(webRootPath, "uploads", "gallery", "docs");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.DocumentFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.DocumentFile.CopyToAsync(stream);
                }

                item.MediaPath = "/uploads/gallery/docs/" + fileName;
            }

            var id = await _galleryService.AddGalleryItemAsync(item);
            return Ok(new { galleryId = id, message = "Gallery item created successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateItem([FromForm] GalleryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new Gallery
            {
                GalleryId = request.GalleryId,
                Title = request.Title,
                Description = request.Description,
                MediaType = request.MediaType,
                Category = request.Category,
                Tags = request.Tags,
                DisplayOrder = request.DisplayOrder,
                IsFeatured = request.IsFeatured,
                VideoEmbedCode = request.VideoEmbedCode,
                IsActive = request.IsActive
            };

            var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Handle Image Upload
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var folder = Path.Combine(webRootPath, "uploads", "gallery", "images");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                item.MediaPath = "/uploads/gallery/images/" + fileName;
                item.ThumbnailPath = "/uploads/gallery/images/" + fileName;
            }

            // Handle Document Upload
            if (request.DocumentFile != null && request.DocumentFile.Length > 0)
            {
                var folder = Path.Combine(webRootPath, "uploads", "gallery", "docs");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.DocumentFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.DocumentFile.CopyToAsync(stream);
                }

                item.MediaPath = "/uploads/gallery/docs/" + fileName;
            }

            await _galleryService.UpdateGalleryItemAsync(item);
            return Ok(new { message = "Gallery item updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _galleryService.DeleteGalleryItemAsync(id);
            return Ok(new { message = "Gallery item deleted successfully" });
        }
    }

    public class GalleryRequest
    {
        public int GalleryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? MediaType { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsFeatured { get; set; }
        public string? VideoEmbedCode { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IFormFile? DocumentFile { get; set; }
    }
}
