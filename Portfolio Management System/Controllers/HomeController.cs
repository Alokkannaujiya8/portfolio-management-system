using Microsoft.AspNetCore.Mvc;
using Portfolio_Management_System.DAL;
using Portfolio_Management_System.Models;
using Portfolio_Management_System.Services;

using System.Diagnostics;

namespace Portfolio_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbHelper _dbHelper;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, DbHelper dbHelper, IEmailService emailService)
        {
            _logger = logger;
            _dbHelper = dbHelper;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new HomeViewModel
                {
                    Profile = await _dbHelper.GetProfileAsync() ?? new ProfileModel(),
                    Skills = await _dbHelper.GetAllSkillsAsync() ?? new List<SkillModel>(),
                    Projects = await _dbHelper.GetAllProjectsAsync() ?? new List<ProjectModel>(),
                    Experience = await _dbHelper.GetAllExperienceAsync() ?? new List<ExperienceModel>(),
                    Education = await _dbHelper.GetAllEducationAsync() ?? new List<EducationModel>(),
                    BlogPosts = await _dbHelper.GetRecentBlogPostsAsync(3) ?? new List<BlogPostModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home page");
                return View(new HomeViewModel());
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ContactMessageModel message)
        {
            try
            {
                Console.WriteLine($"SendMessage called: Name={message.Name}, Email={message.Email}, Subject={message.Subject}, Message={message.Message}");

                if (message == null)
                {
                    Console.WriteLine("Message model is null");
                    return Json(new { success = false, message = "No data received" });
                }

                Console.WriteLine($"Name received: '{message.Name}'");
                Console.WriteLine($"Email received: '{message.Email}'");

                  if (string.IsNullOrWhiteSpace(message.Name))
                {
                    return Json(new { success = false, message = "Name is required" });
                }

                if (string.IsNullOrWhiteSpace(message.Email))
                {
                    return Json(new { success = false, message = "Email is required" });
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = "Validation failed", errors = errors });
                }

                  message.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                message.UserAgent = Request.Headers["User-Agent"].ToString();

                  int messageId = await _dbHelper.SaveContactMessageAsync(message);

                Console.WriteLine($"✅ Message saved successfully with ID: {messageId}");

              
                return Json(new
                {
                    success = true,
                    message = "Message sent successfully",
                    messageId = messageId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        public async Task<IActionResult> DownloadResume()
        {
            try
            {
                var profile = await _dbHelper.GetProfileAsync();
                if (profile?.ResumePath != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", profile.ResumePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                        return File(fileBytes, "application/pdf", "Resume.pdf");
                    }
                }
                return NotFound("Resume not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading resume");
                return NotFound("Error downloading resume");
            }
        }

     

       
      
    }
}