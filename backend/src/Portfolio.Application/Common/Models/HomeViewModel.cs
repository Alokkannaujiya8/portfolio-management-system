using Portfolio.Domain.Entities;
using System.Collections.Generic;

namespace Portfolio.Application.Common.Models
{
    public class HomeViewModel
    {
        public Profile? Profile { get; set; }
        public List<Skill> Skills { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
        public List<Experience> Experience { get; set; } = new();
        public List<Education> Education { get; set; } = new();
        public List<BlogPost> BlogPosts { get; set; } = new();
    }
}
