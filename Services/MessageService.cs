using HMS.Data;
using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all users except current user
        public async Task<List<ApplicationUser>> GetChatUsersAsync(string currentUserId)
        {
            return await _context.Users
                .Where(u => u.Id != currentUserId)
                .ToListAsync();
        }

        // Get all messages between two users
        public async Task<List<Message>> GetChatMessagesAsync(string currentUserId, string otherUserId)
        {
            return await _context.Messages
                .Where(m =>
                    (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                    (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        // Send a new message
        public async Task<Message> SendMessageAsync(string senderId, string receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}
