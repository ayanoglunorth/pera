using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.DataAccess.Abstract
{
    public interface INotificationRepository
    {
        List<Notification> GetByUser(string userId);
        List<Notification> GetUnreadByUser(string userId);
        Notification GetById(int id);
        void Add(Notification notification);
        void MarkAsRead(int id);
        void MarkAllAsRead(string userId);
        int GetUnreadCount(string userId);
    }
}