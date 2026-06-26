using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Portfolio.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IEmailService _emailService;

        public ContactController(IPortfolioService portfolioService, IEmailService emailService)
        {
            _portfolioService = portfolioService;
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ContactMessage message)
        {
            if (message == null)
            {
                return BadRequest(new { success = false, message = "No data received" });
            }

            if (string.IsNullOrWhiteSpace(message.Name))
            {
                return BadRequest(new { success = false, message = "Name is required" });
            }

            if (string.IsNullOrWhiteSpace(message.Email))
            {
                return BadRequest(new { success = false, message = "Email is required" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed", errors = ModelState });
            }

            try
            {
                message.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                message.UserAgent = Request.Headers.UserAgent.ToString();

                var messageId = await _portfolioService.SaveContactMessageAsync(message);

                // Send email notifications in background (don't block the API response on SMTP issues)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendNewMessageNotificationAsync(message);
                        await _emailService.SendContactConfirmationAsync(message.Email, message.Name);
                    }
                    catch
                    {
                        // Log or suppress email sending failures
                    }
                });

                return Ok(new
                {
                    success = true,
                    message = "Message sent successfully",
                    messageId = messageId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _portfolioService.GetAllMessagesAsync();
            return Ok(messages);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("messages/{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _portfolioService.MarkMessageAsReadAsync(id);
            return Ok(new { message = "Message marked as read" });
        }
    }
}
