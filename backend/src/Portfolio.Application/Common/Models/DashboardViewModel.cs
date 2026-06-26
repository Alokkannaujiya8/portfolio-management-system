using Portfolio.Domain.Entities;
using System.Collections.Generic;

namespace Portfolio.Application.Common.Models
{
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
        public List<ContactMessage> RecentMessages { get; set; } = new();
        public List<ResumeView> RecentResumeViews { get; set; } = new();
        public List<VisitorTracking> RecentVisitors { get; set; } = new();
        public List<BlogPost> RecentBlogPosts { get; set; } = new();

        // Chart Data - Visitors Overview (Last 7 days)
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
}
