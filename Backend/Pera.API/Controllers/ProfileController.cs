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
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
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

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? "Student";

            return Ok(new ProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Role = role,
                ProfilePicture = user.ProfilePicture
            });
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdateProfileDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return NotFound();

            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.ProfilePicture = model.ProfilePicture ?? user.ProfilePicture;

            var result = _userManager.UpdateAsync(user).Result;
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Profile updated" });
        }
    }
}