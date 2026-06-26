using System;
using System.Collections.Generic;

namespace Portfolio_Management_System.Models
{
    //public class HomeViewModel
    //{
    //    public ProfileModel? Profile { get; set; }
    //    public List<SkillModel> Skills { get; set; } = new();
    //    public List<ProjectModel> Projects { get; set; } = new();
    //    public List<ExperienceModel> Experience { get; set; } = new();
    //    public List<EducationModel> Education { get; set; } = new();
    //    public List<BlogPostModel> BlogPosts { get; set; } = new();
    //}

    //public class DashboardViewModel
    //{
    //    // Statistics
    //    public int TotalSkills { get; set; }
    //    public int TotalProjects { get; set; }
    //    public int TotalExperience { get; set; }
    //    public int TotalEducation { get; set; }
    //    public int TotalMessages { get; set; }
    //    public int UnreadMessages { get; set; }
    //    public int TotalResumeViews { get; set; }
    //    public int TotalDownloads { get; set; }
    //    public int TotalVisitors { get; set; }
    //    public int TotalPageViews { get; set; }
    //    public int TodayVisitors { get; set; }
    //    public int ActiveChats { get; set; }

    //    // Lists
    //    public List<ContactMessageModel> RecentMessages { get; set; } = new();
    //    public List<ResumeViewModel> RecentResumeViews { get; set; } = new();
    //    public List<VisitorTrackingModel> RecentVisitors { get; set; } = new();

    //    // Chart Data
    //    public string[]? VisitorChartLabels { get; set; }
    //    public int[]? VisitorChartData { get; set; }
    //    public string[]? SourceLabels { get; set; }
    //    public int[]? SourceData { get; set; }
    //}

    public class VisitorTrackingModel
    {
        public int VisitorId { get; set; }
        public string? SessionId { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? ISP { get; set; }
        public string? Organization { get; set; }
        public string? Timezone { get; set; }
        public DateTime FirstVisit { get; set; }
        public DateTime LastVisit { get; set; }
        public int VisitCount { get; set; }
        public string? PagesVisited { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}