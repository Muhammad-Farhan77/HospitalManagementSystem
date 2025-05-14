using HMS.Models;
using Microsoft.AspNetCore.Identity;

namespace HMS.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await EnsureRoleExistsAsync(user.Role);
                await AssignRoleAsync(user, user.Role);
            }
            return result;
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);
            }
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task EnsureRoleExistsAsync(string roleName)
        {
            if (!await RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        public async Task AssignRoleAsync(ApplicationUser user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
