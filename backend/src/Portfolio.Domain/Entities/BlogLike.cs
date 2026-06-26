using System;

namespace Portfolio.Domain.Entities
{
    public class BlogLike
    {
        public int LikeId { get; set; }
        public int? PostId { get; set; }
        public int? VisitorId { get; set; }
        public string? IPAddress { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual BlogPost? BlogPost { get; set; }
    }
}
