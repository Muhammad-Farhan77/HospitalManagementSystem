using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HMS.Models; // Ensure this namespace is correct based on your project structure

namespace HMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // If using Identity for user authentication
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for the Case entity
        public DbSet<Case> Cases { get; set; }

        // DbSet for the Doctor entity
        public DbSet<Doctor> Doctors { get; set; }

        // DbSet for the Patient entity
        public DbSet<Patient> Patients { get; set; }

        // DbSet for the Message entity
        public DbSet<Message> Messages { get; set; }

        // DbSet for the Notification entity
        public DbSet<Notification> Notifications { get; set; }

        // You can add other DbSets for your models here if needed.
    }
}
