using Pera.Business.Abstract;
using Pera.DataAccess.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pera.Business.Concrete
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public List<NotificationDto> GetUserNotifications(string userId)
        {
            var notifications = _notificationRepository.GetByUser(userId);
            return notifications.Select(MapToDto).ToList();
        }

        public List<NotificationDto> GetUnreadNotifications(string userId)
        {
            var notifications = _notificationRepository.GetUnreadByUser(userId);
            return notifications.Select(MapToDto).ToList();
        }

        public int GetUnreadCount(string userId)
        {
            return _notificationRepository.GetUnreadCount(userId);
        }

        public void MarkAsRead(int id, string userId)
        {
            var notification = _notificationRepository.GetById(id);
            if (notification != null && notification.UserId == userId)
            {
                _notificationRepository.MarkAsRead(id);
            }
        }

        public void MarkAllAsRead(string userId)
        {
            _notificationRepository.MarkAllAsRead(userId);
        }

        public void CreateNotification(CreateNotificationDto model)
        {
            var notification = new Notification
            {
                UserId = model.UserId,
                Title = model.Title,
                Description = model.Description,
                Type = model.Type,
                IsRead = false,
                CreatedAt = DateTime.Now
            };
            _notificationRepository.Add(notification);
        }

        private NotificationDto MapToDto(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Description = notification.Description,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt,
                ReadAt = notification.ReadAt
            };
        }
    }
}