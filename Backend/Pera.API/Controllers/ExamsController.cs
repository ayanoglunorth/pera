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
    public class ExamsController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }

        // --- 1. DEFINE EXAM ---
        [HttpPost("define")]
        public IActionResult Define([FromBody] ExamDefinitionDto model)
        {
            _examService.DefineExam(model);
            return Ok(new { message = "Exam definition created." });
        }

        // --- 2. LIST FOR DROPDOWN ---
        [HttpGet("definitions")]
        public IActionResult GetDefinitions([FromQuery] string type = null)
        {
            var result = _examService.GetExamDefinitions(type);
            return Ok(result);
        }

        // --- 3. ADD EXAM RESULT ---
        [HttpPost("add")]
        public IActionResult Add([FromBody] ExamResultEntryDto model)
        {
            if (model.Courses == null || model.Courses.Count == 0)
            {
                return BadRequest("Please enter course results.");
            }

            _examService.EnterExamResult(model);
            return Ok(new { message = "Exam and results saved successfully." });
        }

        // --- 4. STUDENT SEES OWN LIST ---
        [HttpGet("my-list")]
        public IActionResult GetMyList()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = _examService.GetResultsByStudent(userId);
            return Ok(result);
        }

        // --- 5. TEACHER SELECTS STUDENT ---
        [HttpGet("student-results/{studentId}")]
        public IActionResult GetStudentResults(string studentId)
        {
            var result = _examService.GetResultsByStudent(studentId);
            return Ok(result);
        }

        // --- 6. VIEW DETAIL ---
        [HttpGet("detail/{id}")]
        public IActionResult GetDetail(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = _examService.GetExamDetail(id, userId, userRole);

            if (result == null) return NotFound("Exam not found or no permission.");
            return Ok(result);
        }

        // --- 7. DELETE ---
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _examService.DeleteExam(id);
                return Ok(new { message = "Exam deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest("Delete failed: " + ex.Message);
            }
        }

        // --- 8. CLASS AVERAGE ---
        [HttpGet("class-average")]
        public IActionResult GetClassAverage()
        {
            var result = _examService.GetClassAverage();
            return Ok(result);
        }

        [HttpGet("student-list/{examId}")]
        public async Task<IActionResult> GetStudentList(int examId)
        {
            var list = await _examService.GetExamStudentListAsync(examId);
            return Ok(list);
        }

        [HttpPost("bulk-results")]
        public IActionResult SaveBulkResults([FromBody] BulkResultInputDto model)
        {
            _examService.SaveBulkResults(model);
            return Ok(new { message = "Grades saved successfully." });
        }

        [HttpGet("student-report")]
        public IActionResult GetStudentReport()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var results = _examService.GetResultsByStudent(userId);
            return Ok(results);
        }

        [HttpPost("add-result")]
        public IActionResult AddResult([FromBody] ExamResultInputDto model)
        {
            if (model == null || model.Courses == null || model.Courses.Count == 0)
            {
                return BadRequest("Please enter course results.");
            }

            _examService.AddExamResult(model);
            return Ok(new { message = "Student results saved successfully." });
        }
    }
}
