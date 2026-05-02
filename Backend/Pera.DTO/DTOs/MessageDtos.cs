using System;

namespace Pera.DTO.DTOs
{
    // 1. For Sending Messages
    public class SendMessageDto
    {
        public string ReceiverId { get; set; }
        public string Content { get; set; }
    }

    // 2. For Chat Bubbles (Details)
    public class MessageDetailDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsSentByMe { get; set; } // Will it be on right or left?
        public bool IsRead { get; set; }
    }

    // 3. For Left Menu List (Inbox)
    public class ChatBoxDto
    {
        public string UserId { get; set; } // ID of the person I'm chatting with
        public string FullName { get; set; }     // Name of the person I'm chatting with
        public string LastMessage { get; set; }    // "See you..."
        public DateTime LastDate { get; set; }  // 10:42
        public int UnreadCount { get; set; } // Number inside blue circle
    }
}
