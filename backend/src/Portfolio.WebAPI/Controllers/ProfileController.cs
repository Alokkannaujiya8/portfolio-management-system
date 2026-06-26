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
    public class ProfileController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IWebHostEnvironment _env;

        public ProfileController(IPortfolioService portfolioService, IWebHostEnvironment env)
        {
            _portfolioService = portfolioService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _portfolioService.GetProfileAsync();
            if (profile == null)
            {
                return NotFound("Profile not found");
            }
            return Ok(profile);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile = await _portfolioService.GetProfileAsync() ?? new Profile();
            
            profile.Name = request.Name;
            profile.Title = request.Title;
            profile.Description = request.Description;
            profile.Email = request.Email;
            profile.Phone = request.Phone;
            profile.Address = request.Address;
            profile.LinkedIn = request.LinkedIn;
            profile.GitHub = request.GitHub;
            profile.IsActive = true;

            var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Handle Photo Upload
            if (request.PhotoFile != null && request.PhotoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(webRootPath, "uploads", "photos");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.PhotoFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.PhotoFile.CopyToAsync(stream);
                }

                // Delete old photo if exists
                if (!string.IsNullOrEmpty(profile.Photo))
                {
                    var oldFilePath = Path.Combine(webRootPath, profile.Photo.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                profile.Photo = "/uploads/photos/" + fileName;
            }

            // Handle Resume Upload
            if (request.ResumeFile != null && request.ResumeFile.Length > 0)
            {
                var fileExtension = Path.GetExtension(request.ResumeFile.FileName).ToLowerInvariant();
                if (fileExtension != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }

                var uploadsFolder = Path.Combine(webRootPath, "uploads", "resumes");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ResumeFile.CopyToAsync(stream);
                }

                // Delete old resume if exists
                if (!string.IsNullOrEmpty(profile.ResumePath))
                {
                    var oldFilePath = Path.Combine(webRootPath, profile.ResumePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                profile.ResumePath = "/uploads/resumes/" + fileName;
            }

            var profileId = await _portfolioService.UpdateProfileAsync(profile);
            return Ok(new { profileId = profileId, message = "Profile updated successfully" });
        }
    }

    public class ProfileUpdateRequest
    {
        public int ProfileId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public IFormFile? ResumeFile { get; set; }
    }
}
