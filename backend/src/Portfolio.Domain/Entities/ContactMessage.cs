using System;

namespace Portfolio.Domain.Entities
{
    public class ContactMessage : BaseModel
    {
        public int MessageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public bool IsReplied { get; set; }
        public string? ReplyMessage { get; set; }
        public DateTime? RepliedDate { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
