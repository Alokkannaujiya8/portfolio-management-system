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
    public class ProjectsController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IWebHostEnvironment _env;

        public ProjectsController(IPortfolioService portfolioService, IWebHostEnvironment env)
        {
            _portfolioService = portfolioService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _portfolioService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProject([FromForm] ProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = new Project
            {
                ProjectName = request.ProjectName,
                Description = request.Description,
                GitHubLink = request.GitHubLink,
                LiveLink = request.LiveLink,
                IsActive = true
            };

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsFolder = Path.Combine(webRootPath, "uploads", "projects");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                project.ImagePath = "/uploads/projects/" + fileName;
            }

            var projectId = await _portfolioService.AddProjectAsync(project);
            return Ok(new { projectId = projectId, message = "Project added successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromForm] ProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = new Project
            {
                ProjectId = request.ProjectId,
                ProjectName = request.ProjectName,
                Description = request.Description,
                GitHubLink = request.GitHubLink,
                LiveLink = request.LiveLink,
                IsActive = true
            };

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsFolder = Path.Combine(webRootPath, "uploads", "projects");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                project.ImagePath = "/uploads/projects/" + fileName;
            }

            await _portfolioService.UpdateProjectAsync(project);
            return Ok(new { message = "Project updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _portfolioService.DeleteProjectAsync(id);
            return Ok(new { message = "Project deleted successfully" });
        }
    }

    public class ProjectRequest
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? GitHubLink { get; set; }
        public string? LiveLink { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
