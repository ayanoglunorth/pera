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
    public class MesajlarController : ControllerBase
    {
        private readonly IMesajService _mesajService;

        public MesajlarController(IMesajService mesajService)
        {
            _mesajService = mesajService;
        }

        [HttpPost("gonder")]
        public IActionResult Gonder([FromBody] MesajGonderDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _mesajService.MesajGonder(model, userId);
            return Ok(new { message = "Mesaj gönderildi" });
        }

        [HttpGet("inbox")]
        public IActionResult GetInbox()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var liste = _mesajService.GetInbox(userId);
            return Ok(liste);
        }

        [HttpGet("sohbet/{karsiId}")]
        public IActionResult GetSohbet(string karsiId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var detay = _mesajService.GetSohbetDetay(userId, karsiId);
            return Ok(detay);
        }


    }
}