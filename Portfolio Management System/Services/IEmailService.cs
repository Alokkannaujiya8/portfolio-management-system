using Portfolio_Management_System.Models;

namespace Portfolio_Management_System.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailModel email);
        Task SendContactConfirmationAsync(string toEmail, string name);
        Task SendNewMessageNotificationAsync(ContactMessageModel message);
        Task SendResumeDownloadNotificationAsync(ResumeViewModel view);
    }
}