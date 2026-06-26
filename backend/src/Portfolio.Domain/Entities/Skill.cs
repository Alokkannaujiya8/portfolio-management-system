using System;

namespace Portfolio.Domain.Entities
{
    public class Skill : BaseModel
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public int Percentage { get; set; }
    }
}
