using System;

namespace Portfolio.Domain.Entities
{
    public class BlogVideo
    {
        public int VideoId { get; set; }
        public int? PostId { get; set; }
        public string? VideoTitle { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoEmbedCode { get; set; }
        public string? VideoType { get; set; }
        public string? ThumbnailPath { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual BlogPost? BlogPost { get; set; }
    }
}
