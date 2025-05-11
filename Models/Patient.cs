using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public string ApplicationUserId { get; set; } // Foreign key to IdentityUser
        public ApplicationUser User { get; set; }     // Navigation property

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        [Phone]
        public string Mobile { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }

        public ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}
