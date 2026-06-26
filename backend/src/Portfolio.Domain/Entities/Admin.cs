using System;

namespace Portfolio.Domain.Entities
{
    public class Admin : BaseModel
    {
        public int AdminId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
