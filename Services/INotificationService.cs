using HMS.Models;

namespace HMS.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUnreadNotificationsAsync(string userId);
        Task<List<Notification>> GetAllNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task CreateNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsForDoctorBySpecializationAsync(string doctorId, string specialization);
        Task<List<Notification>> GetNotificationsForPatientByCaseAsync(string patientId);
    }
}
