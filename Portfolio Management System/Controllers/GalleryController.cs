using Microsoft.AspNetCore.Mvc;
using Portfolio_Management_System.DAL;
using Portfolio_Management_System.Models;

namespace Portfolio_Management_System.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly DbHelper _dbHelper;

        public GalleryController(ILogger<GalleryController> logger, DbHelper dbHelper)
        {
            _logger = logger;
            _dbHelper = dbHelper;
        }

        // GET: Gallery/Index
        public async Task<IActionResult> Index(string? type)
        {
            try
            {
                var items = await _dbHelper.GetAllGalleryItemsAsync(type);
                var categories = await _dbHelper.GetGalleryCategoriesAsync();

                ViewBag.Categories = categories;
                ViewBag.CurrentType = type;

                return View(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading gallery");
                return View(new List<GalleryModel>());
            }
        }

        // GET: Gallery/Video/{id}
        public async Task<IActionResult> Video(int id)
        {
            try
            {
                var item = await _dbHelper.GetGalleryItemByIdAsync(id);

                if (item == null || item.MediaType != "video")
                {
                    return NotFound();
                }

                // Increment view count
                await _dbHelper.IncrementGalleryViewCountAsync(id);

                return View(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading video: {Id}", id);
                return NotFound();
            }
        }

        // GET: Gallery/Download/{id}
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var item = await _dbHelper.GetGalleryItemByIdAsync(id);

                if (item == null || string.IsNullOrEmpty(item.MediaPath))
                {
                    return NotFound();
                }

                // Increment download count
                await _dbHelper.IncrementGalleryDownloadCountAsync(id);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.MediaPath.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    var fileName = Path.GetFileName(item.MediaPath);
                    return File(fileBytes, "application/octet-stream", fileName);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file: {Id}", id);
                return NotFound();
            }
        }
    }
}