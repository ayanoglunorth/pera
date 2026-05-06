using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pera.Business.Abstract;
using Pera.DTO.DTOs;
using System.Security.Claims;

namespace Pera.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalsController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var goals = _goalService.GetUserGoals(userId);
            return Ok(goals);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var goal = _goalService.GetGoalById(id, userId);
            if (goal == null)
                return NotFound();
            return Ok(goal);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateGoalDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _goalService.CreateGoal(model, userId);
            return Ok(new { message = "Goal created" });
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdateGoalDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _goalService.UpdateGoal(model, userId);
            return Ok(new { message = "Goal updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _goalService.DeleteGoal(id, userId);
            return Ok(new { message = "Goal deleted" });
        }

        [HttpPost("{id}/complete")]
        public IActionResult Complete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _goalService.CompleteGoal(id, userId);
            return Ok(new { message = "Goal completed" });
        }
    }
}