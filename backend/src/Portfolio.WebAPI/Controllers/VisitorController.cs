using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Domain.Entities;
using System.Threading.Tasks;

namespace Portfolio.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public VisitorController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpPost("track")]
        public async Task<IActionResult> TrackVisitor([FromBody] VisitorTracking visitor)
        {
            if (visitor == null) return BadRequest("Invalid request");

            visitor.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            visitor.UserAgent = Request.Headers.UserAgent.ToString();

            var visitorId = await _dashboardService.TrackVisitorAsync(visitor);
            return Ok(new { success = true, visitorId = visitorId });
        }

        [HttpPost("visit")]
        public async Task<IActionResult> TrackPageVisit([FromBody] PageVisit visit)
        {
            if (visit == null) return BadRequest("Invalid request");

            var id = await _dashboardService.AddPageVisitAsync(visit);
            return Ok(new { success = true, pageVisitId = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _dashboardService.GetAllVisitorsAsync();
            return Ok(logs);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/visits")]
        public async Task<IActionResult> GetPageVisits(int id)
        {
            var visits = await _dashboardService.GetPageVisitsByVisitorAsync(id);
            return Ok(visits);
        }
    }
}
