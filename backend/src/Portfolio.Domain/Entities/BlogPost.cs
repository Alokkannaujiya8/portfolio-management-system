using System;
using System.Collections.Generic;
using System.Linq;

namespace Portfolio.Domain.Entities
{
    public class BlogPost : BaseModel
    {
        public int PostId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Excerpt { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImage { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoEmbedCode { get; set; }
        public string? Tags { get; set; }

        // SEO
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        public int? ViewCount { get; set; } = 0;
        public int? LikeCount { get; set; } = 0;
        public int? CommentCount { get; set; } = 0;
        public bool? IsPublished { get; set; } = false;
        public DateTime? PublishedDate { get; set; }
        public bool? IsFeatured { get; set; } = false;

        // Navigation
        public virtual BlogCategory? Category { get; set; }
        public virtual ICollection<BlogImage> Images { get; set; } = new List<BlogImage>();
        public virtual ICollection<BlogVideo> Videos { get; set; } = new List<BlogVideo>();
        public virtual ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();

        // Computed
        public List<string> TagList => string.IsNullOrEmpty(Tags)
            ? new List<string>()
            : Tags.Split(',').Select(t => t.Trim()).ToList();

        public string FormattedDate => PublishedDate?.ToString("MMMM dd, yyyy") ?? CreatedDate.ToString("MMMM dd, yyyy");

        public string ReadingTime
        {
            get
            {
                if (string.IsNullOrEmpty(Content)) return "1 min read";
                var wordCount = Content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                var minutes = Math.Max(1, wordCount / 200);
                return $"{minutes} min read";
            }
        }
    }
}
