using System;

namespace Portfolio.Domain.Entities
{
    public class BlogImage
    {
        public int ImageId { get; set; }
        public int? PostId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string? ThumbnailPath { get; set; }
        public string? Caption { get; set; }
        public string? AltText { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsCover { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual BlogPost? BlogPost { get; set; }
    }
}
