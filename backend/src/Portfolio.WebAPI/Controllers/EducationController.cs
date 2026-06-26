using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Domain.Entities;
using System.Threading.Tasks;

namespace Portfolio.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducationController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public EducationController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEducation()
        {
            var educationList = await _portfolioService.GetAllEducationsAsync();
            return Ok(educationList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddEducation([FromBody] Education education)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _portfolioService.AddEducationAsync(education);
            return Ok(new { educationId = id, message = "Education added successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateEducation([FromBody] Education education)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _portfolioService.UpdateEducationAsync(education);
            return Ok(new { message = "Education updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            await _portfolioService.DeleteEducationAsync(id);
            return Ok(new { message = "Education deleted successfully" });
        }
    }
}
