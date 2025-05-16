using HMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Create a role if it does not exist
        public async Task CreateRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Assign a role to a user if not already assigned
        public async Task AssignRoleToUserAsync(ApplicationUser user, string roleName)
        {
            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        // Optional: Create common default roles
        public async Task SeedDefaultRolesAsync()
        {
            await CreateRoleAsync("Admin");
            await CreateRoleAsync("Doctor");
            await CreateRoleAsync("Patient");
        }
    }
}
