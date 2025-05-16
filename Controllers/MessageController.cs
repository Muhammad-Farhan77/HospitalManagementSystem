using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageController(IMessageService messageService, UserManager<ApplicationUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }

        // GET: List all users to start chat
        public async Task<IActionResult> Users()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var users = await _messageService.GetChatUsersAsync(currentUser.Id);

            return View(users); // View should show a list of users to start chat with
        }

        // GET: Chat between current user and selected user
        public async Task<IActionResult> Chat(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var messages = await _messageService.GetChatMessagesAsync(currentUser.Id, userId);

            var otherUser = await _userManager.FindByIdAsync(userId);
            if (otherUser == null) return NotFound();

            ViewBag.OtherUser = otherUser;
            ViewBag.CurrentUserId = currentUser.Id;

            return View(messages);
        }

        // POST: Send a message
        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, string content)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(receiverId))
                return BadRequest("Message or receiver ID cannot be empty.");

            await _messageService.SendMessageAsync(currentUser.Id, receiverId, content);

            return RedirectToAction("Chat", new { userId = receiverId });
        }
    }
}
