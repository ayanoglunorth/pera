using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System.Security.Claims;

namespace Pera.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public SettingsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return NotFound();

            // Default settings - in a real app, you'd have a Settings entity
            return Ok(new SettingsDto
            {
                UserId = userId,
                EmailNotifications = true,
                ExamReminders = true,
                ResultNotifications = true,
                MessageNotifications = true,
                Theme = "light",
                Language = "tr"
            });
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdateSettingsDto model)
        {
            // In a real app, you'd save these to a Settings entity
            // For now, just return success
            return Ok(new { message = "Settings updated" });
        }
    }
}