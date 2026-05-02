using Pera.DTO.DTOs;
using System.Collections.Generic;

namespace Pera.Business.Abstract
{
    public interface IMessageService
    {
        void SendMessage(SendMessageDto model, string senderId);
        List<MessageDetailDto> GetChatDetail(string myId, string peerId);
        List<ChatBoxDto> GetInbox(string userId);
    }
}
