using Microsoft.EntityFrameworkCore;
using Pera.DataAccess.Abstract;
using Pera.Entity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Pera.DataAccess.Concrete
{
    public class MessageRepository : IMessageRepository
    {
        public void AddMessage(Message message)
        {
            using (var context = new AppDbContext())
            {
                context.Messages.Add(message);
                context.SaveChanges();
            }
        }

        public List<Message> GetChat(string user1Id, string user2Id)
        {
            using (var context = new AppDbContext())
            {
                // Messages between two people (I sent to them OR they sent to me)
                return context.Messages
                    .Include(x => x.Sender) // To get names
                    .Include(x => x.Receiver)
                    .Where(x => (x.SenderId == user1Id && x.ReceiverId == user2Id) ||
                                (x.SenderId == user2Id && x.ReceiverId == user1Id))
                    .OrderBy(x => x.Date) // Oldest to newest
                    .ToList();
            }
        }

        public List<Message> GetAllMyMessages(string userId)
        {
            using (var context = new AppDbContext())
            {
                // Get everything I sent or received
                return context.Messages
                    .Include(x => x.Sender)
                    .Include(x => x.Receiver)
                    .Where(x => x.SenderId == userId || x.ReceiverId == userId)
                    .OrderByDescending(x => x.Date) // Newest on top
                    .ToList();
            }
        }

        public Message GetMessageById(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Messages.FirstOrDefault(x => x.Id == id);
            }
        }

        public void Update(Message message)
        {
            using (var context = new AppDbContext())
            {
                context.Messages.Update(message);
                context.SaveChanges();
            }
        }
    }
}
