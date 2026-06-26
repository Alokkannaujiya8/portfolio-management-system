using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IPortfolioDbContext _context;

        public DashboardService(IPortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            var stats = new DashboardViewModel();

            // 1. Basic Counts
            stats.TotalSkills = await _context.Skills.CountAsync(s => s.IsActive);
            stats.TotalProjects = await _context.Projects.CountAsync(p => p.IsActive);
            
            var experiences = await _context.Experiences.Where(e => e.IsActive).ToListAsync();
            var expDetail = Experience.GetTotalExperienceDetail(experiences);
            stats.TotalExperience = expDetail.YearsOnly;

            stats.TotalEducation = await _context.Educations.CountAsync(e => e.IsActive);
            stats.TotalMessages = await _context.ContactMessages.CountAsync(m => m.IsActive == true);
            stats.UnreadMessages = await _context.ContactMessages.CountAsync(m => m.IsActive == true && !m.IsRead);
            stats.TotalResumeViews = await _context.ResumeViews.CountAsync(r => r.IsActive == true);
            stats.TotalDownloads = await _context.ResumeViews.CountAsync(r => r.IsActive == true && r.IsDownloaded == true);
            stats.TotalVisitors = await _context.VisitorTrackings.CountAsync(v => v.IsActive == true);
            stats.TotalPageViews = await _context.PageVisits.CountAsync(p => p.IsActive == true);
            
            var todayUtc = DateTime.UtcNow.Date;
            stats.TodayVisitors = await _context.PageVisits
                .Where(p => p.VisitTime.HasValue && p.VisitTime.Value.Date == todayUtc && p.IsActive == true)
                .Select(p => p.VisitorId)
                .Distinct()
                .CountAsync();

            var chatCutoff = DateTime.UtcNow.AddDays(-1);
            stats.ActiveChats = await _context.ChatMessages
                .Where(m => m.CreatedDate.HasValue && m.CreatedDate.Value >= chatCutoff && m.IsActive == true)
                .Select(m => m.VisitorId)
                .Distinct()
                .CountAsync();

            stats.TotalBlogPosts = await _context.BlogPosts.CountAsync(p => p.IsActive);
            stats.TotalGalleryItems = await _context.Galleries.CountAsync(g => g.IsActive);
            stats.PendingComments = await _context.BlogComments.CountAsync(c => c.IsActive == true && c.IsApproved != true);

            // 2. Recent Lists (Take 5)
            stats.RecentMessages = await _context.ContactMessages
                .Where(m => m.IsActive == true)
                .OrderByDescending(m => m.CreatedDate)
                .Take(5)
                .ToListAsync();

            stats.RecentResumeViews = await _context.ResumeViews
                .Where(r => r.IsActive == true)
                .OrderByDescending(r => r.ViewDate)
                .Take(5)
                .ToListAsync();

            stats.RecentVisitors = await _context.VisitorTrackings
                .Where(v => v.IsActive == true)
                .OrderByDescending(v => v.LastVisit)
                .Take(5)
                .ToListAsync();

            stats.RecentBlogPosts = await _context.BlogPosts
                .Include(b => b.Category)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedDate)
                .Take(5)
                .ToListAsync();

            // 3. Visitors Chart Data (Last 7 Days)
            var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6);
            var pageVisits = await _context.PageVisits
                .Where(p => p.VisitTime.HasValue && p.VisitTime.Value.Date >= sevenDaysAgo && p.IsActive == true)
                .ToListAsync();

            var visitorGroups = pageVisits
                .GroupBy(p => p.VisitTime!.Value.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            for (int i = 0; i < 7; i++)
            {
                var date = sevenDaysAgo.AddDays(i);
                stats.VisitorChartLabels[i] = date.ToString("MMM dd");
                stats.VisitorChartData[i] = visitorGroups.GetValueOrDefault(date, 0);
            }

            // 4. Traffic Sources (Last 30 Days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var userAgents = await _context.VisitorTrackings
                .Where(v => v.CreatedDate.HasValue && v.CreatedDate.Value >= thirtyDaysAgo && v.IsActive == true)
                .Select(v => v.UserAgent)
                .ToListAsync();

            int bots = userAgents.Count(ua => ua != null && ua.Contains("bot", StringComparison.OrdinalIgnoreCase));
            int mobile = userAgents.Count(ua => ua != null && !ua.Contains("bot", StringComparison.OrdinalIgnoreCase) && ua.Contains("mobile", StringComparison.OrdinalIgnoreCase));
            int desktop = userAgents.Count(ua => ua != null && !ua.Contains("bot", StringComparison.OrdinalIgnoreCase) && !ua.Contains("mobile", StringComparison.OrdinalIgnoreCase));

            stats.SourceLabels = new[] { "Desktop", "Mobile", "Bots" };
            stats.SourceData = new[] { desktop, mobile, bots };

            // 5. Resume Downloads Chart Data (Last 7 Days)
            var resumeViews = await _context.ResumeViews
                .Where(r => r.ViewDate.HasValue && r.ViewDate.Value.Date >= sevenDaysAgo && r.IsActive == true && r.IsDownloaded == true)
                .ToListAsync();

            var resumeGroups = resumeViews
                .GroupBy(r => r.ViewDate!.Value.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            for (int i = 0; i < 7; i++)
            {
                var date = sevenDaysAgo.AddDays(i);
                stats.ResumeChartLabels[i] = date.ToString("MMM dd");
                stats.ResumeChartData[i] = resumeGroups.GetValueOrDefault(date, 0);
            }

            // 6. Popular Pages
            stats.PopularPages = await _context.PageVisits
                .Where(p => p.IsActive == true && p.PageUrl != null)
                .GroupBy(p => new { p.PageUrl, p.PageTitle })
                .Select(g => new PopularPageModel
                {
                    PageUrl = g.Key.PageUrl!,
                    PageName = g.Key.PageTitle ?? g.Key.PageUrl!,
                    ViewCount = g.Count(),
                    Trend = "+5%"
                })
                .OrderByDescending(x => x.ViewCount)
                .Take(5)
                .ToListAsync();

            return stats;
        }

        public async Task<int> TrackVisitorAsync(VisitorTracking visitor)
        {
            var existing = await _context.VisitorTrackings
                .FirstOrDefaultAsync(v => v.SessionId == visitor.SessionId && v.IsActive == true);

            if (existing != null)
            {
                existing.LastVisit = DateTime.UtcNow;
                existing.VisitCount = (existing.VisitCount ?? 0) + 1;
                
                if (!string.IsNullOrEmpty(visitor.PagesVisited))
                {
                    existing.PagesVisited = string.IsNullOrEmpty(existing.PagesVisited)
                        ? visitor.PagesVisited
                        : existing.PagesVisited + "," + visitor.PagesVisited;
                }
                
                existing.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existing.VisitorId;
            }
            else
            {
                visitor.FirstVisit = DateTime.UtcNow;
                visitor.LastVisit = DateTime.UtcNow;
                visitor.VisitCount = 1;
                visitor.IsActive = true;
                visitor.CreatedDate = DateTime.UtcNow;
                visitor.UpdatedDate = DateTime.UtcNow;
                
                _context.VisitorTrackings.Add(visitor);
                await _context.SaveChangesAsync();
                return visitor.VisitorId;
            }
        }

        public async Task<VisitorTracking?> GetVisitorByIdAsync(int id)
        {
            return await _context.VisitorTrackings.FirstOrDefaultAsync(v => v.VisitorId == id && v.IsActive == true);
        }

        public async Task<List<VisitorTracking>> GetAllVisitorsAsync()
        {
            return await _context.VisitorTrackings
                .Where(v => v.IsActive == true)
                .OrderByDescending(v => v.LastVisit)
                .ToListAsync();
        }

        public async Task<int> AddPageVisitAsync(PageVisit visit)
        {
            visit.VisitTime = DateTime.UtcNow;
            visit.CreatedDate = DateTime.UtcNow;
            visit.IsActive = true;

            _context.PageVisits.Add(visit);
            await _context.SaveChangesAsync();
            return visit.PageVisitId;
        }

        public async Task<List<PageVisit>> GetPageVisitsByVisitorAsync(int visitorId)
        {
            return await _context.PageVisits
                .Where(pv => pv.VisitorId == visitorId && pv.IsActive == true)
                .OrderByDescending(pv => pv.VisitTime)
                .ToListAsync();
        }

        public async Task<int> TrackResumeViewAsync(ResumeView view)
        {
            view.ViewDate = DateTime.UtcNow;
            view.CreatedDate = DateTime.UtcNow;
            view.IsActive = true;

            _context.ResumeViews.Add(view);
            await _context.SaveChangesAsync();
            return view.ViewId;
        }

        public async Task<List<ResumeView>> GetAllResumeViewsAsync()
        {
            return await _context.ResumeViews
                .Where(r => r.IsActive == true)
                .OrderByDescending(r => r.ViewDate)
                .ToListAsync();
        }
    }
}
