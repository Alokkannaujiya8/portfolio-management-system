using System;

namespace Portfolio.Domain.Entities
{
    public class Profile : BaseModel
    {
        public int ProfileId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public string? Photo { get; set; }
        public string? ResumePath { get; set; }
    }
}
