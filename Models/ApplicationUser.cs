using Microsoft.AspNetCore.Identity;

namespace HMS.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public string Role { get; set; } // Admin, Doctor, Patient
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
public enum Role
{
    Admin = 10,
    Doctor = 20,
    Patient = 30
}
}

