using System;

namespace Portfolio.Domain.Entities
{
    public class VisitorTracking
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
        public DateTime? FirstVisit { get; set; }
        public DateTime? LastVisit { get; set; }
        public int? VisitCount { get; set; }
        public string? PagesVisited { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
