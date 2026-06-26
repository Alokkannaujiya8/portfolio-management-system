using Microsoft.AspNetCore.Mvc;
using Portfolio_Management_System.DAL;
using Portfolio_Management_System.Models;
using System.Text.Json;

namespace Portfolio_Management_System.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ILogger<ResumeController> _logger;
        private readonly DbHelper _dbHelper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;

        
        public ResumeController(
            ILogger<ResumeController> logger,
            DbHelper dbHelper,
            IWebHostEnvironment hostEnvironment,
            HttpClient httpClient)
        {
            _logger = logger;
            _dbHelper = dbHelper;
            _hostEnvironment = hostEnvironment;
            _httpClient = httpClient;
        }

    
        public IActionResult ViewResume()
        {
            return View(new ResumeViewModel());
        }

        // POST: Resume/ViewResume - Form submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewResume(ResumeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                model.UserAgent = Request.Headers.UserAgent.ToString();
                model.ViewDate = DateTime.Now;
                model.IsDownloaded = true;
                model.DownloadDate = DateTime.Now;
                model.IsActive = true;
                model.CreatedDate = DateTime.Now;

                await _dbHelper.TrackResumeViewAsync(model);
                return await DownloadResumeFile();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading resume");
                ModelState.AddModelError("", "Error downloading resume. Please try again.");
                return View(model);
            }
        }


        public async Task<IActionResult> Download()
        {
            return await DownloadResumeFile();
        }

    
        private async Task<IActionResult> DownloadResumeFile()
        {
            try
            {
                var profile = await _dbHelper.GetProfileAsync();

                if (profile?.ResumePath == null)
                {
                    return NotFound("Resume not found. Please upload a resume first.");
                }

                var filePath = Path.Combine(_hostEnvironment.WebRootPath, profile.ResumePath.TrimStart('/'));

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("Resume file not found on server.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var fileName = Path.GetFileName(profile.ResumePath);

                return File(fileBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading resume file");
                return NotFound("Error downloading resume");
            }
        }

        // API to get user location
        [HttpGet]
        public async Task<IActionResult> GetUserLocation()
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();


                if (ipAddress == "::1" || ipAddress == "127.0.0.1")
                {
                    return Json(new { country = "India", city = "Mumbai" });
                }

             
                var response = await _httpClient.GetAsync($"http://ip-api.com/json/{ipAddress}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var location = JsonSerializer.Deserialize<IpApiResponse>(json);

                    if (location != null && location.Status == "success")
                    {
                        return Json(new { country = location.Country, city = location.City });
                    }
                }

                return Json(new { country = "", city = "" });
            }
            catch
            {
                return Json(new { country = "", city = "" });
            }
        }

        public class IpApiResponse
        {
            public string Status { get; set; } = "";
            public string Country { get; set; } = "";
            public string City { get; set; } = "";
        }
    }
}