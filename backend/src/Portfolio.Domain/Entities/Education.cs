using System;

namespace Portfolio.Domain.Entities
{
    public class Education : BaseModel
    {
        public int EducationId { get; set; }
        public string Degree { get; set; } = string.Empty;
        public string Institute { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal? Percentage { get; set; }
    }
}
