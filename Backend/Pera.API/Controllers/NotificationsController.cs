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
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var notifications = _notificationService.GetUserNotifications(userId);
            return Ok(notifications);
        }

        [HttpGet("unread")]
        public IActionResult GetUnread()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var notifications = _notificationService.GetUnreadNotifications(userId);
            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        public IActionResult GetUnreadCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var count = _notificationService.GetUnreadCount(userId);
            return Ok(new { count });
        }

        [HttpPost("{id}/read")]
        public IActionResult MarkAsRead(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _notificationService.MarkAsRead(id, userId);
            return Ok(new { message = "Marked as read" });
        }

        [HttpPost("read-all")]
        public IActionResult MarkAllAsRead()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _notificationService.MarkAllAsRead(userId);
            return Ok(new { message = "All marked as read" });
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateNotificationDto model)
        {
            _notificationService.CreateNotification(model);
            return Ok(new { message = "Notification created" });
        }
    }
}