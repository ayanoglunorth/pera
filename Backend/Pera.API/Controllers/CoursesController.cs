using Microsoft.AspNetCore.Mvc;
using Pera.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Pera.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("grades")]
        public IActionResult GetGrades()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User identity not found.");
            }

            var data = _courseService.GetStudentCourseGrades(userId);
            return Ok(data);
        }
    }
}
