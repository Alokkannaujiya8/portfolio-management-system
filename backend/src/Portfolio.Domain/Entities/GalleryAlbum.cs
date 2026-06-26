using System;
using System.Collections.Generic;

namespace Portfolio.Domain.Entities
{
    public class GalleryAlbum : BaseModel
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public string? Slug { get; set; }
        public int? DisplayOrder { get; set; } = 0;

        // Navigation property for album items
        public virtual ICollection<AlbumItem> AlbumItems { get; set; } = new List<AlbumItem>();
    }
}
