using System;

namespace Portfolio.Domain.Entities
{
    public class ResumeView
    {
        public int ViewId { get; set; }
        public string? VisitorName { get; set; }
        public string? VisitorEmail { get; set; }
        public string? CompanyName { get; set; }
        public string? Designation { get; set; }
        public DateTime? ViewDate { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public bool? IsDownloaded { get; set; }
        public DateTime? DownloadDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
