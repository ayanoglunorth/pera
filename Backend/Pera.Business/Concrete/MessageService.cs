using Pera.Business.Abstract;
using Pera.DataAccess.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pera.Business.Concrete
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public void SendMessage(SendMessageDto model, string senderId)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = model.ReceiverId,
                Content = model.Content,
                Date = DateTime.Now,
                IsRead = false
            };
            _messageRepository.AddMessage(message);
        }

        public List<MessageDetailDto> GetChatDetail(string myId, string peerId)
        {
            var messages = _messageRepository.GetChat(myId, peerId);

            // If message came to me and is unread, mark as "Read"
            foreach (var m in messages)
            {
                if (m.ReceiverId == myId && !m.IsRead)
                {
                    m.IsRead = true;
                    _messageRepository.Update(m);
                }
            }

            return messages.Select(x => new MessageDetailDto
            {
                Id = x.Id,
                SenderId = x.SenderId,
                SenderName = x.Sender.UserName,
                Content = x.Content,
                Date = x.Date,
                IsSentByMe = (x.SenderId == myId),
                IsRead = x.IsRead
            }).ToList();
        }

        public List<ChatBoxDto> GetInbox(string userId)
        {
            // 1. Fetch all my messages
            var allMessages = _messageRepository.GetAllMyMessages(userId);

            // 2. Group by person I'm chatting with
            // Who is the other side of the message? (If I'm sender, it's receiver; if I'm receiver, it's sender)
            var chats = allMessages
                .GroupBy(x => x.SenderId == userId ? x.ReceiverId : x.SenderId)
                .Select(group => new
                {
                    PeerId = group.Key,
                    // Get latest message in group
                    LastMessage = group.OrderByDescending(m => m.Date).First(),
                    // Count of unread messages sent to me
                    UnreadCount = group.Count(m => m.ReceiverId == userId && !m.IsRead)
                })
                .ToList();

            // 3. Convert to DTO
            var result = chats.Select(s => new ChatBoxDto
            {
                UserId = s.PeerId,

                // We need to find peer's name. We can get from last message.
                // If I sent last message, it's Receiver's name, if they sent, it's Sender's name.
                FullName = (s.LastMessage.SenderId == userId)
                          ? s.LastMessage.Receiver.UserName
                          : s.LastMessage.Sender.UserName,

                LastMessage = s.LastMessage.Content,
                LastDate = s.LastMessage.Date,
                UnreadCount = s.UnreadCount
            })
            .OrderByDescending(x => x.LastDate) // Most recent chat on top
            .ToList();

            return result;
        }
    }
}
