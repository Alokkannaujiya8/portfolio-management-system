using System;

namespace Portfolio.Domain.Entities
{
    public class Project : BaseModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public string? GitHubLink { get; set; }
        public string? LiveLink { get; set; }
    }
}
