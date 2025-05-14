using HMS.Data;
using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetAllNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetNotificationsForDoctorBySpecializationAsync(string doctorId, string specialization)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == doctorId)
                .ToListAsync();

            return notifications
                .Where(n => n.Message != null && n.Message.Contains(specialization, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<Notification>> GetNotificationsForPatientByCaseAsync(string patientId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == patientId)
                .ToListAsync();

            return notifications
                .Where(n => n.Message != null && n.Message.Contains("case", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
