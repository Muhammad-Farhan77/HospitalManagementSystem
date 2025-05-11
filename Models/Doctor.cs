namespace HMS.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ApplicationUserId { get; set; } // Foreign key


        public ApplicationUser User { get; set; }

        public string Specialization { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

        public string Email { get; set; }
        public int Mobile { get; set; }

        public ICollection<Case> Cases { get; set; }
    }
   
}
