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
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send")]
        public IActionResult Send([FromBody] SendMessageDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _messageService.SendMessage(model, userId);
            return Ok(new { message = "Message sent" });
        }

        [HttpGet("inbox")]
        public IActionResult GetInbox()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var list = _messageService.GetInbox(userId);
            return Ok(list);
        }

        [HttpGet("chat/{peerId}")]
        public IActionResult GetChat(string peerId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var detail = _messageService.GetChatDetail(userId, peerId);
            return Ok(detail);
        }
    }
}
