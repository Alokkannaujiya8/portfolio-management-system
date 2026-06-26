using MailKit.Net.Smtp;
using MimeKit;
using Portfolio_Management_System.Models;
using Microsoft.Extensions.Options;

namespace Portfolio_Management_System.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(EmailModel email)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", email.ToEmail));
                message.Subject = email.Subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = email.Body;
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, _emailSettings.EnableSsl);

                if (!string.IsNullOrEmpty(_emailSettings.Username))
                {
                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent to {Email}", email.ToEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", email.ToEmail);
                throw;
            }
        }

        public async Task SendContactConfirmationAsync(string toEmail, string name)
        {
            var email = new EmailModel
            {
                ToEmail = toEmail,
                Subject = "Thank you for contacting me",
                Body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Hi {name},</h2>
                    <p>Thank you for reaching out. I have received your message and will get back to you soon.</p>
                    <p>Best regards,<br>Your Name</p>
                </body>
                </html>"
            };

            await SendEmailAsync(email);
        }

        public async Task SendNewMessageNotificationAsync(ContactMessageModel message)
        {
            var email = new EmailModel
            {
                ToEmail = _emailSettings.SenderEmail,
                Subject = $"New Message: {message.Subject}",
                Body = $@"
                <html>
                <body>
                    <h2>New Message from {message.Name}</h2>
                    <p><strong>Email:</strong> {message.Email}</p>
                    <p><strong>Subject:</strong> {message.Subject}</p>
                    <p><strong>Message:</strong> {message.Message}</p>
                </body>
                </html>"
            };

            await SendEmailAsync(email);
        }

        public async Task SendResumeDownloadNotificationAsync(ResumeViewModel view)
        {
            var email = new EmailModel
            {
                ToEmail = _emailSettings.SenderEmail,
                Subject = "Resume Downloaded",
                Body = $@"
                <html>
                <body>
                    <h2>Resume Download Notification</h2>
                    <p><strong>Name:</strong> {view.VisitorName}</p>
                    <p><strong>Email:</strong> {view.VisitorEmail}</p>
                    <p><strong>Company:</strong> {view.CompanyName}</p>
                    <p><strong>Designation:</strong> {view.Designation}</p>
                    <p><strong>Time:</strong> {view.DownloadDate:yyyy-MM-dd HH:mm:ss}</p>
                </body>
                </html>"
            };

            await SendEmailAsync(email);
        }
    }
}