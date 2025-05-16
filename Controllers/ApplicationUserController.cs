using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserService _userService;

        public ApplicationUserController(IApplicationUserService userService)
        {
            _userService = userService;
        }

        // GET: /ApplicationUser/
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // GET: /ApplicationUser/Create
        public IActionResult Create()
        {
            ViewBag.Roles = Enum.GetNames(typeof(Role));
            return View();
        }

        // POST: /ApplicationUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser model, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Role = model.Role,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userService.CreateUserAsync(user, password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            ViewBag.Roles = Enum.GetNames(typeof(Role));
            return View(model);
        }

        // GET: /ApplicationUser/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.Roles = Enum.GetNames(typeof(Role));
            return View(user);
        }

        // POST: /ApplicationUser/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser updatedUser)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.UserName = updatedUser.Email;
            user.Role = updatedUser.Role;

            var result = await _userService.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.Roles = Enum.GetNames(typeof(Role));
            return View(user);
        }

        // GET: /ApplicationUser/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: /ApplicationUser/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result.Succeeded)
            {
                // Handle delete errors if needed, e.g. add model state error
                ModelState.AddModelError("", "Failed to delete user.");
                var user = await _userService.GetUserByIdAsync(id);
                return View("Delete", user);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
