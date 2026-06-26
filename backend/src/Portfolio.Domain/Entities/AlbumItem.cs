using System;

namespace Portfolio.Domain.Entities
{
    public class AlbumItem
    {
        public int AlbumItemId { get; set; }
        public int? AlbumId { get; set; }
        public int? GalleryId { get; set; }
        public int? DisplayOrder { get; set; } = 0;
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual GalleryAlbum? Album { get; set; }
        public virtual Gallery? Gallery { get; set; }
    }
}
