using System;
using System.Collections.Generic;

namespace Portfolio.Domain.Entities
{
    public class BlogCategory : BaseModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int? DisplayOrder { get; set; }

        // Navigation property
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
