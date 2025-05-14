using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix multiple cascade paths
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Case>()
       .HasOne(c => c.Patient)
       .WithMany()
       .HasForeignKey(c => c.PatientId)
       .OnDelete(DeleteBehavior.Restrict); // Or NoAction

            modelBuilder.Entity<Case>()
                .HasOne(c => c.Doctor)
                .WithMany()
                .HasForeignKey(c => c.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // You can add other DbSets for your models here if needed.
    }
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Replace this connection string with your actual connection string
            optionsBuilder.UseSqlServer("Server=DESKTOP-VP4C8UT;Database=HMS;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
