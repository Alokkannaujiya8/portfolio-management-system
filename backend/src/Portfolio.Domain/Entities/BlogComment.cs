using System;
using System.Collections.Generic;

namespace Portfolio.Domain.Entities
{
    public class BlogComment
    {
        public int CommentId { get; set; }
        public int? PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public int? VisitorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool? IsApproved { get; set; } = false;
        public bool? IsSpam { get; set; } = false;
        public string? IPAddress { get; set; }
        public int? LikeCount { get; set; } = 0;
        public bool? IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; }

        public virtual BlogPost? BlogPost { get; set; }
        public virtual BlogComment? ParentComment { get; set; }
        public virtual ICollection<BlogComment> Replies { get; set; } = new List<BlogComment>();
    }
}
