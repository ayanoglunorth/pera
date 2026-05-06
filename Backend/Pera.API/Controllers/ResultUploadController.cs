using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pera.Business.Abstract;
using Pera.DTO.DTOs;
using System;
using System.IO;
using System.Security.Claims;

namespace Pera.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class ResultUploadController : ControllerBase
    {
        private readonly IUploadedResultService _uploadedResultService;

        public ResultUploadController(IUploadedResultService uploadedResultService)
        {
            _uploadedResultService = uploadedResultService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(teacherId))
                return Unauthorized();

            var results = _uploadedResultService.GetAll(teacherId);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(teacherId))
                return Unauthorized();

            var result = _uploadedResultService.GetById(id, teacherId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}/download")]
        public IActionResult Download(int id)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(teacherId))
                return Unauthorized();

            var filePath = _uploadedResultService.GetFilePath(id, teacherId);
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                return NotFound();

            var fileName = Path.GetFileName(filePath);
            var mimeType = "application/octet-stream";
            return PhysicalFile(Path.GetFullPath(filePath), mimeType, fileName);
        }

        [HttpPost]
        public IActionResult Post([FromForm] UploadResultDto model)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(teacherId))
                return Unauthorized();

            if (model.File == null || model.File.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            _uploadedResultService.Upload(model, teacherId);
            return Ok(new { message = "File uploaded" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(teacherId))
                return Unauthorized();

            _uploadedResultService.Delete(id, teacherId);
            return Ok(new { message = "File deleted" });
        }
    }
}