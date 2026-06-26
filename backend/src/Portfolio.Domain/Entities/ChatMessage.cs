using System;

namespace Portfolio.Domain.Entities
{
    public class ChatMessage
    {
        public int ChatId { get; set; }
        public int? VisitorId { get; set; }
        public string? VisitorName { get; set; }
        public string? VisitorEmail { get; set; }
        public string? Message { get; set; }
        public bool? IsFromAdmin { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public string? IPAddress { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
