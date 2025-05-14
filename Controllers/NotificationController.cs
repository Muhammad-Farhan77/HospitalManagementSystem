using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(NotificationService notificationService, UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        // GET: All notifications for the current user
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var notifications = await _notificationService.GetAllNotificationsAsync(currentUser.Id);

            return View(notifications);
        }

        // POST: Mark a notification as read
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Create a new notification
        [HttpPost]
        public async Task<IActionResult> Create(string userId, string title, string message)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
                return BadRequest();

            var newNotification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationService.CreateNotificationAsync(newNotification);
            return Ok("Notification sent.");
        }
    }
}
