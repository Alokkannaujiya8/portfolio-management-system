using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Domain.Entities;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portfolio.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IPortfolioService _portfolioService;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _httpClient;

        public ResumeController(
            IDashboardService dashboardService,
            IPortfolioService portfolioService,
            IEmailService emailService,
            IWebHostEnvironment env,
            HttpClient httpClient)
        {
            _dashboardService = dashboardService;
            _portfolioService = portfolioService;
            _emailService = emailService;
            _env = env;
            _httpClient = httpClient;
        }

        [HttpPost("track")]
        public async Task<IActionResult> TrackView([FromBody] ResumeView view)
        {
            if (view == null) return BadRequest("Invalid request");

            try
            {
                view.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                view.UserAgent = Request.Headers.UserAgent.ToString();
                view.ViewDate = DateTime.UtcNow;
                view.IsDownloaded = true;
                view.DownloadDate = DateTime.UtcNow;
                view.IsActive = true;
                view.CreatedDate = DateTime.UtcNow;

                var viewId = await _dashboardService.TrackResumeViewAsync(view);

                // Send email in background
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendResumeDownloadNotificationAsync(view);
                    }
                    catch
                    {
                        // Suppress
                    }
                });

                return Ok(new { success = true, viewId = viewId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download()
        {
            try
            {
                var profile = await _portfolioService.GetProfileAsync();
                if (profile?.ResumePath == null)
                {
                    return NotFound("Resume not found. Please upload a resume first.");
                }

                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var filePath = Path.Combine(webRootPath, profile.ResumePath.TrimStart('/'));

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
                return StatusCode(500, new { message = $"Error downloading resume: {ex.Message}" });
            }
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetUserLocation()
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (ipAddress == "::1" || ipAddress == "127.0.0.1" || string.IsNullOrEmpty(ipAddress))
                {
                    return Ok(new { country = "India", city = "Mumbai" });
                }

                var response = await _httpClient.GetAsync($"http://ip-api.com/json/{ipAddress}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var location = JsonSerializer.Deserialize<IpApiResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (location != null && location.Status == "success")
                    {
                        return Ok(new { country = location.Country, city = location.City });
                    }
                }

                return Ok(new { country = "", city = "" });
            }
            catch
            {
                return Ok(new { country = "", city = "" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _dashboardService.GetAllResumeViewsAsync();
            return Ok(logs);
        }

        private class IpApiResponse
        {
            public string Status { get; set; } = "";
            public string Country { get; set; } = "";
            public string City { get; set; } = "";
        }
    }
}
