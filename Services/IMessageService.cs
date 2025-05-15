using HMS.Models;

namespace HMS.Services
{
    public interface IMessageService
    {
        Task<List<ApplicationUser>> GetChatUsersAsync(string currentUserId);
        Task<List<Message>> GetChatMessagesAsync(string currentUserId, string otherUserId);
        Task<Message> SendMessageAsync(string senderId, string receiverId, string content);
    }
}
