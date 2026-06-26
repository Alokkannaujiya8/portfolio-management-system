using System;

namespace Portfolio.Domain.Entities
{
    public class PageVisit
    {
        public int PageVisitId { get; set; }
        public int? VisitorId { get; set; }
        public string? PageUrl { get; set; }
        public string? PageTitle { get; set; }
        public int? TimeSpent { get; set; }
        public DateTime? VisitTime { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
