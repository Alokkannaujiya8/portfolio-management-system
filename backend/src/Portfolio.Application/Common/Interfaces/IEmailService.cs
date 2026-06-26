using Portfolio.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Application.Common.Interfaces
{
    // Define the email models here or use domain entities
    public class EmailModel
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        // In WebAPI, attachments are represented as uploaded files or byte streams
        public List<AttachmentModel>? Attachments { get; set; }
    }

    public class AttachmentModel
    {
        public byte[] Content { get; set; } = [];
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }

    public interface IEmailService
    {
        Task SendEmailAsync(EmailModel email);
        Task SendContactConfirmationAsync(string toEmail, string name);
        Task SendNewMessageNotificationAsync(ContactMessage message);
        Task SendResumeDownloadNotificationAsync(ResumeView view);
    }
}
