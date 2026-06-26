using Microsoft.AspNetCore.Mvc;
using Portfolio_Management_System.DAL;
using Portfolio_Management_System.Models;

namespace Portfolio_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly DbHelper _dbHelper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<AdminController> _logger;

        public AdminController(DbHelper dbHelper, IWebHostEnvironment hostEnvironment, ILogger<AdminController> logger)
        {
            _dbHelper = dbHelper;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // ==================== AUTHENTICATION ====================

        // GET: Admin/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isValid = await _dbHelper.ValidateAdminAsync(model.Username, model.Password);

                    if (isValid)
                    {
                        HttpContext.Session.SetString("Admin", model.Username);
                        HttpContext.Session.SetString("AdminLoginTime", DateTime.Now.ToString());
                        return RedirectToAction("Dashboard");
                    }

                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
                ModelState.AddModelError("", "Database connection error. Please try again.");
            }

            return View(model);
        }

        // GET: Admin/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ==================== DASHBOARD ====================

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login");
            }

            try
            {
                var stats = await _dbHelper.GetDashboardStatsAsync();

                // Calculate accurate total experience
                var experiences = await _dbHelper.GetAllExperienceAsync();
                var experienceDetail = ExperienceModel.GetTotalExperienceDetail(experiences);

                // Update the TotalExperience with accurate years
                stats.TotalExperience = experienceDetail.YearsOnly;

                ViewBag.Username = HttpContext.Session.GetString("Admin");
                ViewBag.ExperienceDetail = experienceDetail;

                return View(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dashboard error");
                TempData["Error"] = "Error loading dashboard";
                return View(new DashboardViewModel());
            }
        }

        // GET: Admin/GetExperienceDetails (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetExperienceDetails()
        {
            try
            {
                if (HttpContext.Session.GetString("Admin") == null)
                {
                    return Unauthorized(new { success = false, message = "Unauthorized" });
                }

                var experiences = await _dbHelper.GetAllExperienceAsync();
                var detail = ExperienceModel.GetTotalExperienceDetail(experiences);

                return Json(new
                {
                    success = true,
                    totalMonths = detail.TotalMonths,
                    formatted = detail.Formatted,
                    currentFormatted = detail.CurrentFormatted,
                    pastFormatted = detail.PastFormatted,
                    yearsOnly = detail.YearsOnly,
                    currentYears = detail.CurrentYears,
                    currentMonths = detail.CurrentMonths,
                    pastYears = detail.PastYears,
                    pastMonths = detail.PastMonths
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting experience details");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ==================== PROFILE ====================

        // GET: Admin/Profile
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login");
            }

            var profile = await _dbHelper.GetProfileAsync();
            return View(profile ?? new ProfileModel());
        }

        // POST: Admin/Profile
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileModel profile, IFormFile? photoFile, IFormFile? resumeFile)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login");

            try
            {
                // Handle Photo Upload
                if (photoFile != null && photoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "photos");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(stream);
                    }

                    // Delete old photo if exists
                    if (!string.IsNullOrEmpty(profile.Photo))
                    {
                        var oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, profile.Photo.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    profile.Photo = "/uploads/photos/" + fileName;
                }

                // Handle Resume Upload
                if (resumeFile != null && resumeFile.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".pdf" };
                    var fileExtension = Path.GetExtension(resumeFile.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Only PDF files are allowed.");
                        return View(profile);
                    }

                    // Validate file size (max 5MB)
                    if (resumeFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "File size must be less than 5MB.");
                        return View(profile);
                    }

                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "resumes");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await resumeFile.CopyToAsync(stream);
                    }

                    // Delete old resume if exists
                    if (!string.IsNullOrEmpty(profile.ResumePath))
                    {
                        var oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, profile.ResumePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    profile.ResumePath = "/uploads/resumes/" + fileName;
                }

                await _dbHelper.UpdateProfileAsync(profile);
                TempData["Success"] = "Profile updated successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profile update error");
                TempData["Error"] = "Error updating profile: " + ex.Message;
            }

            return RedirectToAction("Profile");
        }

        // ==================== SKILLS ====================

        // GET: Admin/Skills
        [HttpGet]
        public async Task<IActionResult> Skills()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login");
            }

            var skills = await _dbHelper.GetAllSkillsAsync();
            return View(skills);
        }

        // POST: Admin/AddSkill (AJAX)
        [HttpPost]
        public async Task<IActionResult> AddSkill([FromBody] SkillModel skill)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Unauthorized(new { success = false, message = "Unauthorized" });
            }

            try
            {
                var id = await _dbHelper.AddSkillAsync(skill);
                return Ok(new { success = true, id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add skill error");
                return StatusCode(500, new { success = false, message = "Error adding skill" });
            }
        }

        // POST: Admin/UpdateSkill (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateSkill([FromBody] SkillModel skill)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Unauthorized(new { success = false, message = "Unauthorized" });
            }

            try
            {
                await _dbHelper.UpdateSkillAsync(skill);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update skill error");
                return StatusCode(500, new { success = false });
            }
        }

        // POST: Admin/DeleteSkill (AJAX)
        [HttpPost]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Unauthorized(new { success = false, message = "Unauthorized" });
            }

            try
            {
                await _dbHelper.DeleteSkillAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete skill error");
                return StatusCode(500, new { success = false });
            }
        }

        // ==================== PROJECTS ====================

        // GET: Admin/Projects
        public async Task<IActionResult> Projects()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login");
            }

            var projects = await _dbHelper.GetAllProjectsAsync();
            return View(projects);
        }

        // POST: Admin/AddProject (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProject([FromForm] ProjectModel project, IFormFile? imageFile)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(project.ProjectName))
                {
                    return Json(new { success = false, message = "Project name is required" });
                }

                // Handle Image Upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return Json(new { success = false, message = "Only image files (jpg, jpeg, png, gif, webp) are allowed." });
                    }

                    // Validate file size (max 2MB)
                    if (imageFile.Length > 2 * 1024 * 1024)
                    {
                        return Json(new { success = false, message = "Image size must be less than 2MB." });
                    }

                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "projects");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    project.ImagePath = "/uploads/projects/" + fileName;
                }

                // Set default values
                project.IsActive = true;
                project.CreatedDate = DateTime.Now;
                project.UpdatedDate = DateTime.Now;

                // Save to database
                int projectId = await _dbHelper.AddProjectAsync(project);

                _logger.LogInformation($"Project added successfully with ID: {projectId}");

                return Json(new { success = true, message = "Project added successfully!", id = projectId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add project error");
                return Json(new { success = false, message = "Error adding project: " + ex.Message });
            }
        }

        // POST: Admin/UpdateProject (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject([FromForm] ProjectModel project, IFormFile? imageFile)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            try
            {
                // Validate required fields
                if (project.ProjectId <= 0)
                {
                    return Json(new { success = false, message = "Invalid project ID" });
                }

                if (string.IsNullOrWhiteSpace(project.ProjectName))
                {
                    return Json(new { success = false, message = "Project name is required" });
                }

                // Get existing project
                var existingProjects = await _dbHelper.GetAllProjectsAsync();
                var existingProject = existingProjects.FirstOrDefault(p => p.ProjectId == project.ProjectId);

                if (existingProject == null)
                {
                    return Json(new { success = false, message = "Project not found" });
                }

                // Handle Image Upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return Json(new { success = false, message = "Only image files (jpg, jpeg, png, gif, webp) are allowed." });
                    }

                    // Validate file size (max 2MB)
                    if (imageFile.Length > 2 * 1024 * 1024)
                    {
                        return Json(new { success = false, message = "Image size must be less than 2MB." });
                    }

                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingProject.ImagePath))
                    {
                        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, existingProject.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            _logger.LogInformation($"Deleted old project image: {oldImagePath}");
                        }
                    }

                    // Upload new image
                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "projects");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    project.ImagePath = "/uploads/projects/" + fileName;
                }
                else
                {
                    // Keep existing image
                    project.ImagePath = existingProject.ImagePath;
                }

                // Update timestamp
                project.UpdatedDate = DateTime.Now;
                project.IsActive = true;

                // Save to database
                await _dbHelper.UpdateProjectAsync(project);

                _logger.LogInformation($"Project updated successfully with ID: {project.ProjectId}");

                return Json(new { success = true, message = "Project updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update project error");
                return Json(new { success = false, message = "Error updating project: " + ex.Message });
            }
        }

        // POST: Admin/DeleteProject (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Unauthorized(new { success = false, message = "Unauthorized" });
            }

            try
            {
                // Get project to delete its image
                var projects = await _dbHelper.GetAllProjectsAsync();
                var project = projects.FirstOrDefault(p => p.ProjectId == id);

                if (project != null && !string.IsNullOrEmpty(project.ImagePath))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, project.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                        _logger.LogInformation($"Deleted project image: {imagePath}");
                    }
                }

                await _dbHelper.DeleteProjectAsync(id);

                _logger.LogInformation($"Project deleted successfully with ID: {id}");

                return Ok(new { success = true, message = "Project deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete project error");
                return StatusCode(500, new { success = false, message = "Error deleting project: " + ex.Message });
            }
        }

        // ==================== EXPERIENCE ====================

        // GET: Admin/Experience
        public async Task<IActionResult> Experience()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login");

            var experiences = await _dbHelper.GetAllExperienceAsync();

            // Calculate experience details for each item
            foreach (var exp in experiences)
            {
                // The Duration and TotalMonths properties will be calculated automatically
                // by the ExperienceModel
            }

            return View(experiences);
        }

        // POST: Admin/AddExperience
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExperience([FromForm] ExperienceModel experience)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false, message = "Unauthorized" });

            try
            {
                // Validate dates
                if (experience.StartDate > DateTime.Now)
                {
                    return BadRequest(new { success = false, message = "Start date cannot be in the future" });
                }

                if (experience.EndDate.HasValue && experience.EndDate < experience.StartDate)
                {
                    return BadRequest(new { success = false, message = "End date cannot be before start date" });
                }

                experience.CreatedDate = DateTime.Now;
                experience.UpdatedDate = DateTime.Now;
                experience.IsActive = true;

                var id = await _dbHelper.AddExperienceAsync(experience);
                return Ok(new { success = true, id = id, message = "Experience added successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add experience error");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/UpdateExperience
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateExperience([FromForm] ExperienceModel experience)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false, message = "Unauthorized" });

            try
            {
                // Validate dates
                if (experience.StartDate > DateTime.Now)
                {
                    return BadRequest(new { success = false, message = "Start date cannot be in the future" });
                }

                if (experience.EndDate.HasValue && experience.EndDate < experience.StartDate)
                {
                    return BadRequest(new { success = false, message = "End date cannot be before start date" });
                }

                experience.UpdatedDate = DateTime.Now;
                await _dbHelper.UpdateExperienceAsync(experience);
                return Ok(new { success = true, message = "Experience updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update experience error");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/DeleteExperience
        [HttpPost]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                await _dbHelper.DeleteExperienceAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete experience error");
                return StatusCode(500, new { success = false });
            }
        }

        // ==================== EDUCATION ====================

        // GET: Admin/Education
        public async Task<IActionResult> Education()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Login");

            var educations = await _dbHelper.GetAllEducationAsync();
            return View(educations);
        }

        // POST: Admin/AddEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEducation([FromForm] EducationModel education)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false, message = "Unauthorized" });

            try
            {
                education.CreatedDate = DateTime.Now;
                education.UpdatedDate = DateTime.Now;
                education.IsActive = true;

                var id = await _dbHelper.AddEducationAsync(education);
                return Ok(new { success = true, id = id, message = "Education added successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add education error");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/UpdateEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEducation([FromForm] EducationModel education)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false, message = "Unauthorized" });

            try
            {
                education.UpdatedDate = DateTime.Now;
                await _dbHelper.UpdateEducationAsync(education);
                return Ok(new { success = true, message = "Education updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update education error");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/DeleteEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return Unauthorized(new { success = false });

            try
            {
                await _dbHelper.DeleteEducationAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete education error");
                return StatusCode(500, new { success = false });
            }
        }

        // ==================== MESSAGES ====================

        // GET: Admin/Messages
        public async Task<IActionResult> Messages()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login");
            }

            var messages = await _dbHelper.GetAllMessagesAsync();
            return View(messages);
        }

        // POST: Admin/MarkMessageAsRead (AJAX)
        [HttpPost]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Unauthorized(new { success = false });
            }

            try
            {
                await _dbHelper.MarkMessageAsReadAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mark message as read error");
                return StatusCode(500, new { success = false });
            }
        }

        // GET: Admin/GetRecentMessages
        [HttpGet]
        public async Task<IActionResult> GetRecentMessages()
        {
            try
            {
                var messages = await _dbHelper.GetAllMessagesAsync();
                var recentMessages = messages.OrderByDescending(m => m.CreatedDate)
                                             .Take(5)
                                             .Select(m => new
                                             {
                                                 m.MessageId,
                                                 m.Name,
                                                 m.Email,
                                                 m.Subject,
                                                 m.Message,
                                                 m.IsRead,
                                                 m.CreatedDate
                                             })
                                             .ToList();

                return Json(recentMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent messages");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: Admin/GetUnreadMessagesCount
        [HttpGet]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            try
            {
                if (HttpContext.Session.GetString("Admin") == null)
                {
                    return Unauthorized(0);
                }

                var messages = await _dbHelper.GetAllMessagesAsync();
                var unreadCount = messages.Count(m => !m.IsRead);

                return Json(unreadCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread messages count");
                return StatusCode(500, 0);
            }
        }

        // ==================== RESUME VIEWS ====================

        public async Task<IActionResult> ResumeViews()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login");
            }

            var views = await _dbHelper.GetAllResumeViewsAsync();
            return View(views);
        }

        // ==================== TEST CONNECTION ====================

        // GET: Admin/TestConnection
        public async Task<IActionResult> TestConnection()
        {
            var result = await _dbHelper.TestConnectionAsync();
            return Content(result ? "✅ Database connection successful!" : "❌ Database connection failed!");
        }
    }
}