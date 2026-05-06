using Pera.DTO.DTOs;
using System.Collections.Generic;

namespace Pera.Business.Abstract
{
    public interface INotificationService
    {
        List<NotificationDto> GetUserNotifications(string userId);
        List<NotificationDto> GetUnreadNotifications(string userId);
        int GetUnreadCount(string userId);
        void MarkAsRead(int id, string userId);
        void MarkAllAsRead(string userId);
        void CreateNotification(CreateNotificationDto model);
    }
}