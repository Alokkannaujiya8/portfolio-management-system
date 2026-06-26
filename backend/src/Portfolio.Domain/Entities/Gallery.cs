using System;
using System.Collections.Generic;

namespace Portfolio.Domain.Entities
{
    public class Gallery : BaseModel
    {
        public int GalleryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? MediaType { get; set; } // image, video, document
        public string? MediaPath { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? VideoEmbedCode { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }
        public int? DisplayOrder { get; set; } = 0;
        public bool? IsFeatured { get; set; } = false;
        public int? ViewCount { get; set; } = 0;
        public int? DownloadCount { get; set; } = 0;

        // Navigation property for album items
        public virtual ICollection<AlbumItem> AlbumItems { get; set; } = new List<AlbumItem>();
    }
}
