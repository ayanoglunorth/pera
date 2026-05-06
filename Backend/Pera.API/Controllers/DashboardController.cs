using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pera.Business.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System.Linq;
using System.Security.Claims;

namespace Pera.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGoalService _goalService;
        private readonly IExamService _examService;

        public DashboardController(
            UserManager<AppUser> userManager,
            IGoalService goalService,
            IExamService examService)
        {
            _userManager = userManager;
            _goalService = goalService;
            _examService = examService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst("role")?.Value ?? User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (role == "Teacher")
            {
                return Ok(GetTeacherDashboard(userId));
            }
            else
            {
                return Ok(GetStudentDashboard(userId));
            }
        }

        private DashboardDto GetTeacherDashboard(string teacherId)
        {
            var students = _userManager.GetUsersInRoleAsync("Student").Result;
            var examDefinitions = _examService.GetExamDefinitions();
            var classAvg = _examService.GetClassAverage();

            return new DashboardDto
            {
                StudentCount = students.Count,
                ClassAverage = classAvg.Any() ? classAvg.Average(x => x.Score ?? 0) : 0,
                PendingResults = 0
            };
        }

        private DashboardDto GetStudentDashboard(string studentId)
        {
            var goals = _goalService.GetUserGoals(studentId);
            var results = _examService.GetResultsByStudent(studentId);
            var upcomingExams = _examService.GetExamDefinitions();

            return new DashboardDto
            {
                TotalExams = results.Count,
                AverageScore = results.Any() ? (decimal)results.Average(x => x.Score ?? 0) : 0,
                TotalGoals = goals.Count,
                CompletedGoals = goals.Count(g => g.IsCompleted),
                UpcomingExams = upcomingExams.Take(5).ToList(),
                RecentResults = results.Take(5).ToList(),
                Goals = goals.Take(5).ToList()
            };
        }
    }
}