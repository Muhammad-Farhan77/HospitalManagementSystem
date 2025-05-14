using HMS.Models;
using Microsoft.AspNetCore.Identity;

namespace HMS.Services
{
    public interface IApplicationUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<bool> RoleExistsAsync(string roleName);
        Task EnsureRoleExistsAsync(string roleName);
        Task AssignRoleAsync(ApplicationUser user, string roleName);
    }
}
