using System.ComponentModel.DataAnnotations;

namespace Portfolio_Management_System.Models
{
    // ==================== BASE MODEL ====================
    public class BaseModel
    {
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    // ==================== ADMIN MODELS ====================
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    // ==================== PROFILE MODEL ====================
    public class ProfileModel : BaseModel
    {
        public int ProfileId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        [Display(Name = "Professional Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "About Me")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Url]
        public string? LinkedIn { get; set; }

        [Url]
        public string? GitHub { get; set; }

        public string? Photo { get; set; }
        public string? ResumePath { get; set; }
    }

    // ==================== SKILL MODEL ====================
    public class SkillModel : BaseModel
    {
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Skill name is required")]
        [StringLength(100)]
        public string SkillName { get; set; } = string.Empty;

        [Required]
        [Range(0, 100)]
        public int Percentage { get; set; }
    }

    // ==================== PROJECT MODEL ====================
    public class ProjectModel : BaseModel
    {
        public int ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProjectName { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        [Url]
        public string? GitHubLink { get; set; }

        [Url]
        public string? LiveLink { get; set; }
    }

    // ==================== EXPERIENCE MODEL ====================
    // ==================== EXPERIENCE MODEL ====================
    // Models/AllModels.cs
    // ==================== EXPERIENCE MODEL ====================
    public class ExperienceModel : BaseModel
    {
        public int ExperienceId { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Role { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        // Display duration for timeline (e.g., "Jan 2023 - Present")
        public string SimpleDuration => EndDate.HasValue
            ? $"{StartDate:MMM yyyy} - {EndDate:MMM yyyy}"
            : $"{StartDate:MMM yyyy} - Present";

        // Calculate duration for a single experience (e.g., "2 years 3 months")
        public string Duration
        {
            get
            {
                var end = EndDate ?? DateTime.Now;
                var start = StartDate;

                int totalMonths = ((end.Year - start.Year) * 12) + end.Month - start.Month;

                if (end.Day < start.Day)
                {
                    totalMonths--;
                }

                if (totalMonths <= 0) return "Less than a month";

                int years = totalMonths / 12;
                int months = totalMonths % 12;

                if (years > 0 && months > 0)
                    return $"{years} year{(years > 1 ? "s" : "")} {months} month{(months > 1 ? "s" : "")}";
                else if (years > 0)
                    return $"{years} year{(years > 1 ? "s" : "")}";
                else
                    return $"{months} month{(months > 1 ? "s" : "")}";
            }
        }

        // Calculate total months for a single experience
        public int TotalMonths
        {
            get
            {
                var end = EndDate ?? DateTime.Now;
                var start = StartDate;

                int months = ((end.Year - start.Year) * 12) + end.Month - start.Month;

                if (end.Day < start.Day)
                {
                    months--;
                }

                return Math.Max(0, months);
            }
        }

        // ==================== STATIC METHODS FOR TOTAL EXPERIENCE ====================

        /// <summary>
        /// Get total months from all experiences (simple sum - may count overlapping periods twice)
        /// </summary>
        public static int GetTotalMonthsFromAll(List<ExperienceModel> experiences)
        {
            if (experiences == null || !experiences.Any())
                return 0;

            int totalMonths = 0;
            foreach (var exp in experiences)
            {
                totalMonths += exp.TotalMonths;
            }
            return totalMonths;
        }

        /// <summary>
        /// Get total months from all experiences with overlapping periods merged
        /// This gives accurate total experience without double-counting overlapping jobs
        /// </summary>
        public static int GetTotalMonthsMerged(List<ExperienceModel> experiences)
        {
            if (experiences == null || !experiences.Any())
                return 0;

            var sortedExperiences = experiences
                .Where(e => e.IsActive)
                .OrderBy(e => e.StartDate)
                .ToList();

            var mergedRanges = new List<(DateTime Start, DateTime End)>();

            foreach (var exp in sortedExperiences)
            {
                DateTime start = exp.StartDate;
                DateTime end = exp.EndDate ?? DateTime.Now;

                if (!mergedRanges.Any())
                {
                    mergedRanges.Add((start, end));
                }
                else
                {
                    var last = mergedRanges.Last();
                    if (start <= last.End)
                    {
                        // Overlapping, merge
                        mergedRanges[mergedRanges.Count - 1] = (last.Start, last.End > end ? last.End : end);
                    }
                    else
                    {
                        mergedRanges.Add((start, end));
                    }
                }
            }

            int totalMonths = 0;
            foreach (var range in mergedRanges)
            {
                totalMonths += (int)((range.End - range.Start).TotalDays / 30.44);
            }

            return totalMonths;
        }

        /// <summary>
        /// Get total years from all experiences (simple sum)
        /// </summary>
        public static double GetTotalYearsFromAll(List<ExperienceModel> experiences)
        {
            return Math.Round(GetTotalMonthsFromAll(experiences) / 12.0, 1);
        }

        /// <summary>
        /// Get total years with overlapping periods merged
        /// </summary>
        public static double GetTotalYearsMerged(List<ExperienceModel> experiences)
        {
            return Math.Round(GetTotalMonthsMerged(experiences) / 12.0, 1);
        }

        /// <summary>
        /// Get formatted total experience string (simple sum)
        /// </summary>
        public static string GetTotalExperienceString(List<ExperienceModel> experiences)
        {
            int totalMonths = GetTotalMonthsFromAll(experiences);
            return FormatDuration(totalMonths);
        }

        /// <summary>
        /// Get formatted total experience string with merged overlapping periods
        /// </summary>
        public static string GetTotalExperienceStringMerged(List<ExperienceModel> experiences)
        {
            int totalMonths = GetTotalMonthsMerged(experiences);
            return FormatDuration(totalMonths);
        }

        /// <summary>
        /// Get detailed experience information including current job and past jobs
        /// </summary>
        public static TotalExperienceDetail GetTotalExperienceDetail(List<ExperienceModel> experiences)
        {
            var detail = new TotalExperienceDetail();

            if (experiences == null || !experiences.Any())
                return detail;

            // Calculate total merged months
            detail.TotalMonths = GetTotalMonthsMerged(experiences);

            // Calculate current job months (job with no EndDate)
            var currentJob = experiences.FirstOrDefault(e => !e.EndDate.HasValue && e.IsActive);
            if (currentJob != null)
            {
                detail.CurrentTotalMonths = currentJob.TotalMonths;
            }

            // Calculate past jobs months
            var pastJobs = experiences.Where(e => e.EndDate.HasValue && e.IsActive).ToList();
            var pastMonthsMerged = GetTotalMonthsMerged(pastJobs);
            detail.PastTotalMonths = pastMonthsMerged;

            return detail;
        }

        /// <summary>
        /// Get current job experience only
        /// </summary>
        public static (int Years, int Months, int TotalMonths) GetCurrentExperience(List<ExperienceModel> experiences)
        {
            if (experiences == null || !experiences.Any())
                return (0, 0, 0);

            var currentJob = experiences.FirstOrDefault(e => !e.EndDate.HasValue && e.IsActive);

            if (currentJob == null)
                return (0, 0, 0);

            int totalMonths = currentJob.TotalMonths;
            return (totalMonths / 12, totalMonths % 12, totalMonths);
        }

        /// <summary>
        /// Get list of experiences with their individual durations
        /// </summary>
        public static List<ExperienceDuration> GetExperiencesWithDuration(List<ExperienceModel> experiences)
        {
            if (experiences == null || !experiences.Any())
                return new List<ExperienceDuration>();

            return experiences
                .Where(e => e.IsActive)
                .Select(e => new ExperienceDuration
                {
                    CompanyName = e.CompanyName,
                    Role = e.Role,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Duration = e.Duration,
                    DurationMonths = e.TotalMonths,
                    SimpleDuration = e.SimpleDuration
                })
                .OrderByDescending(e => e.StartDate)
                .ToList();
        }

        /// <summary>
        /// Format months into readable string
        /// </summary>
        private static string FormatDuration(int totalMonths)
        {
            if (totalMonths <= 0) return "No experience";

            int years = totalMonths / 12;
            int months = totalMonths % 12;

            if (years > 0 && months > 0)
                return $"{years} year{(years > 1 ? "s" : "")} {months} month{(months > 1 ? "s" : "")}";
            else if (years > 0)
                return $"{years} year{(years > 1 ? "s" : "")}";
            else
                return $"{months} month{(months > 1 ? "s" : "")}";
        }
    }

    /// <summary>
    /// Detailed experience information class
    /// </summary>
    public class TotalExperienceDetail
    {
        public int TotalMonths { get; set; }
        public int CurrentTotalMonths { get; set; }
        public int PastTotalMonths { get; set; }

        public int Years => TotalMonths / 12;
        public int Months => TotalMonths % 12;

        public int CurrentYears => CurrentTotalMonths / 12;
        public int CurrentMonths => CurrentTotalMonths % 12;

        public int PastYears => PastTotalMonths / 12;
        public int PastMonths => PastTotalMonths % 12;

        public string Formatted => TotalMonths > 0
            ? $"{Years} year{(Years > 1 ? "s" : "")} {Months} month{(Months > 1 ? "s" : "")}"
            : "No experience";

        public string CurrentFormatted => CurrentTotalMonths > 0
            ? CurrentYears > 0
                ? $"{CurrentYears} year{(CurrentYears > 1 ? "s" : "")} {CurrentMonths} month{(CurrentMonths > 1 ? "s" : "")}"
                : $"{CurrentMonths} month{(CurrentMonths > 1 ? "s" : "")}"
            : "No current job";

        public string PastFormatted => PastTotalMonths > 0
            ? PastYears > 0
                ? $"{PastYears} year{(PastYears > 1 ? "s" : "")} {PastMonths} month{(PastMonths > 1 ? "s" : "")}"
                : $"{PastMonths} month{(PastMonths > 1 ? "s" : "")}"
            : "No past experience";

        // For display in badge (e.g., "5+")
        public int YearsOnly => Years;

        // For detailed breakdown
        public string DetailedBreakdown => $"{Formatted} total ({CurrentFormatted} current, {PastFormatted} past)";
    }

    /// <summary>
    /// Experience duration helper class
    /// </summary>
    public class ExperienceDuration
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Duration { get; set; } = string.Empty;
        public int DurationMonths { get; set; }
        public string SimpleDuration { get; set; } = string.Empty;
        public bool IsCurrent => !EndDate.HasValue;
    }
    //public class ExperienceModel : BaseModel
    //{
    //    public int ExperienceId { get; set; }

    //    [Required]
    //    [StringLength(200)]
    //    public string CompanyName { get; set; } = string.Empty;

    //    [Required]
    //    [StringLength(100)]
    //    public string Role { get; set; } = string.Empty;

    //    [Required]
    //    [DataType(DataType.Date)]
    //    public DateTime StartDate { get; set; }

    //    [DataType(DataType.Date)]
    //    public DateTime? EndDate { get; set; }

    //    [DataType(DataType.MultilineText)]
    //    public string Description { get; set; } = string.Empty;

    //    public string Duration => EndDate.HasValue
    //        ? $"{StartDate:MMM yyyy} - {EndDate:MMM yyyy}"
    //        : $"{StartDate:MMM yyyy} - Present";
    //}

    // ==================== EDUCATION MODEL ====================
    public class EducationModel : BaseModel
    {
        public int EducationId { get; set; }

        [Required]
        [StringLength(100)]
        public string Degree { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Institute { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Range(0, 100)]
        public decimal? Percentage { get; set; }
    }

    // ==================== CONTACT MESSAGE MODEL ====================
    public class ContactMessageModel : BaseModel
    {
        public int MessageId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }
        public bool IsReplied { get; set; }
        public string? ReplyMessage { get; set; }
        public DateTime? RepliedDate { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
    }

    // ==================== RESUME VIEW MODEL ====================
    public class ResumeViewModel
    {
        public int ViewId { get; set; }
        public string? VisitorName { get; set; }
        public string? VisitorEmail { get; set; }
        public string? CompanyName { get; set; }
        public string? Designation { get; set; }
        public DateTime ViewDate { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public bool IsDownloaded { get; set; }
        public DateTime? DownloadDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // ==================== BLOG CATEGORY MODEL ====================
    public class BlogCategoryModel : BaseModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
        public int PostCount { get; set; }
    }
    // ==================== DASHBOARD VIEW MODEL ====================
    public class DashboardViewModel
    {
        // Statistics
        public int TotalSkills { get; set; }
        public int TotalProjects { get; set; }
        public int TotalExperience { get; set; }
        public int TotalEducation { get; set; }
        public int TotalMessages { get; set; }
        public int UnreadMessages { get; set; }
        public int TotalResumeViews { get; set; }
        public int TotalDownloads { get; set; }
        public int TotalVisitors { get; set; }
        public int TotalPageViews { get; set; }
        public int TodayVisitors { get; set; }
        public int ActiveChats { get; set; }
        public int TotalBlogPosts { get; set; }
        public int TotalGalleryItems { get; set; }
        public int PendingComments { get; set; }

        // Lists
        public List<ContactMessageModel> RecentMessages { get; set; } = new();
        public List<ResumeViewModel> RecentResumeViews { get; set; } = new();
        public List<VisitorTrackingModel> RecentVisitors { get; set; } = new();
        public List<BlogPostModel> RecentBlogPosts { get; set; } = new();

        // Chart Data - Visitors Overview
        public string[] VisitorChartLabels { get; set; } = new string[7];
        public int[] VisitorChartData { get; set; } = new int[7];

        // Chart Data - Traffic Sources
        public string[] SourceLabels { get; set; } = new string[3];
        public int[] SourceData { get; set; } = new int[3];

        // Chart Data - Resume Downloads (Last 7 days)
        public string[] ResumeChartLabels { get; set; } = new string[7];
        public int[] ResumeChartData { get; set; } = new int[7];

        // Popular Pages
        public List<PopularPageModel> PopularPages { get; set; } = new();
    }

    public class PopularPageModel
    {
        public string PageName { get; set; } = string.Empty;
        public string PageUrl { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public string Trend { get; set; } = "+0%";
    }
  
    public class BlogImageModel
    {
        public int ImageId { get; set; }
        public int PostId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string? ThumbnailPath { get; set; }
        public string? Caption { get; set; }
        public string? AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsCover { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // ==================== BLOG VIDEO MODEL ====================
    public class BlogVideoModel
    {
        public int VideoId { get; set; }
        public int PostId { get; set; }
        public string? VideoTitle { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoEmbedCode { get; set; }
        public string? VideoType { get; set; }
        public string? ThumbnailPath { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // ==================== BLOG COMMENT MODEL ====================
    public class BlogCommentModel
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public int? VisitorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public bool IsSpam { get; set; }
        public string? IPAddress { get; set; }
        public int LikeCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? PostTitle { get; set; }
        // Additional Properties
        public string? Country { get; set; }
        public string? City { get; set; }
        public List<BlogCommentModel> Replies { get; set; } = new();
    }

    // ==================== BLOG POST VIEW MODEL (NEW) ====================
    public class BlogPostViewModel
    {
        public BlogPostModel Post { get; set; } = new();
        public List<BlogCategoryModel> Categories { get; set; } = new();
        public List<BlogPostModel> RelatedPosts { get; set; } = new();
        public List<BlogPostModel> PopularPosts { get; set; } = new();
        public BlogCommentModel NewComment { get; set; } = new();
        public bool HasUserLiked { get; set; }
    }

   
    public class BlogPostModel : BaseModel
    {
        public int PostId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Excerpt { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? FeaturedImage { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoEmbedCode { get; set; }
        public string? Tags { get; set; }

        // SEO Properties
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
        public bool IsFeatured { get; set; }
        public string? PostTitle { get; set; }
        // Navigation Properties
        public BlogCategoryModel? Category { get; set; }
        public List<BlogImageModel> Images { get; set; } = new();
        public List<BlogVideoModel> Videos { get; set; } = new();
        public List<BlogCommentModel> Comments { get; set; } = new();

        // Computed Properties
        public List<string> TagList => string.IsNullOrEmpty(Tags)
            ? new List<string>()
            : Tags.Split(',').Select(t => t.Trim()).ToList();

        public string FormattedDate => PublishedDate?.ToString("MMMM dd, yyyy") ?? CreatedDate.ToString("MMMM dd, yyyy");

        public string ReadingTime
        {
            get
            {
                var wordCount = Content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                var minutes = Math.Max(1, wordCount / 200);
                return $"{minutes} min read";
            }
        }

        public int ImageCount { get; set; }
        public int VideoCount { get; set; }
        public int ApprovedCommentCount { get; set; }
    }
    // ==================== GALLERY MODEL ====================
    public class GalleryModel : BaseModel
    {
        public int GalleryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? MediaType { get; set; } // image, video, document
        public string? MediaPath { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? VideoEmbedCode { get; set; } // For YouTube/Vimeo embed
        public string? Category { get; set; }
        public string? Tags { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsFeatured { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
    }
    // ==================== HOME VIEW MODEL ====================
    public class HomeViewModel
    {
        public ProfileModel? Profile { get; set; }
        public List<SkillModel> Skills { get; set; } = new();
        public List<ProjectModel> Projects { get; set; } = new();
        public List<ExperienceModel> Experience { get; set; } = new();
        public List<EducationModel> Education { get; set; } = new();
        public List<BlogPostModel> BlogPosts { get; set; } = new();
    }

    // ==================== DASHBOARD VIEW MODEL ====================
    //public class DashboardViewModel
    //{
    //    public int TotalSkills { get; set; }
    //    public int TotalProjects { get; set; }
    //    public int TotalExperience { get; set; }
    //    public int TotalEducation { get; set; }
    //    public int TotalMessages { get; set; }
    //    public int UnreadMessages { get; set; }
    //    public int TotalResumeViews { get; set; }
    //    public int TotalDownloads { get; set; }

    //    public List<ContactMessageModel> RecentMessages { get; set; } = new();
    //    public List<ResumeViewModel> RecentResumeViews { get; set; } = new();

    //    public string[] VisitorChartLabels { get; set; } = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    //    public int[] VisitorChartData { get; set; } = new int[7];
    //    public string[] SourceLabels { get; set; } = { "Direct", "Social", "Search" };
    //    public int[] SourceData { get; set; } = new int[3];
    //}

    // ==================== EMAIL MODELS ====================
    public class EmailModel
    {
        [Required, EmailAddress]
        public string ToEmail { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
        public List<IFormFile>? Attachments { get; set; }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }

    // ==================== ERROR VIEW MODEL ====================
    
}