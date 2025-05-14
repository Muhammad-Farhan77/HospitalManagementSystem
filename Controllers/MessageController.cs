using HMS.Data;
using HMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: List all users to start chat
        public async Task<IActionResult> Users()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var users = await _context.Users
                .Where(u => u.Id != currentUser.Id)
                .ToListAsync();

            return View(users); // View should show a list of users to start chat with
        }

        // GET: Chat between current user and selected user
        public async Task<IActionResult> Chat(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var otherUser = await _userManager.FindByIdAsync(userId);
            if (otherUser == null) return NotFound();

            var messages = await _context.Messages
                .Where(m =>
                    (m.SenderId == currentUser.Id && m.ReceiverId == userId) ||
                    (m.SenderId == userId && m.ReceiverId == currentUser.Id))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            ViewBag.OtherUser = otherUser;
            ViewBag.CurrentUserId = currentUser.Id;

            return View(messages);
        }

        // POST: Send a message
        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, string content)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(receiverId))
                return BadRequest("Message or receiver ID cannot be empty.");

            var message = new Message
            {
                Content = content,
                SenderId = currentUser.Id,
                ReceiverId = receiverId,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { userId = receiverId });
        }
    }
}
