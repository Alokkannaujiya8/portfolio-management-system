using Portfolio.Application.Common.Models;
using Portfolio.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Application.Common.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardStatsAsync();
        
        // Visitor Analytics Tracking
        Task<int> TrackVisitorAsync(VisitorTracking visitor);
        Task<VisitorTracking?> GetVisitorByIdAsync(int id);
        Task<List<VisitorTracking>> GetAllVisitorsAsync();
        Task<int> AddPageVisitAsync(PageVisit visit);
        Task<List<PageVisit>> GetPageVisitsByVisitorAsync(int visitorId);

        // Resume Views Logging
        Task<int> TrackResumeViewAsync(ResumeView view);
        Task<List<ResumeView>> GetAllResumeViewsAsync();
    }
}
