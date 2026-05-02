using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.DataAccess.Abstract
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);

        // Get all conversation between two people
        List<Message> GetChat(string user1Id, string user2Id);

        // Get ALL messages involving a user (needed for Inbox)
        List<Message> GetAllMyMessages(string userId);

        // Mark message as read
        Message GetMessageById(int id);
        void Update(Message message);
    }
}
