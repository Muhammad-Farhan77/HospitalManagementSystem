namespace HMS.Models
{
    public class Case
    {
        public int CaseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key to Patient
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }

        // Medical Report List
        public List<string> SelectedConditions { get; set; } = new List<string>();
        public List<IFormFile> CaseDocuments { get; set; } = new List<IFormFile>();
        public List<IFormFile> CaseImages { get; set; } = new List<IFormFile>();


        public List<string> AvailableCondition { get; set; } = new List<string>
    {
        "Diabetes",
        "Hypertension",
        "Asthma",
        "Cancer",
        "Heart Disease",
        "Kidney Disease"
    }; // Case Status (Pending, Approved, Rejected, etc.)
        public string DoctorComments { get; set; } // Doctor writes findings, notes here
        public string PrescribedMedicines { get; set; } // Example: "Panadol 500mg, twice a day"
        public DateTime? ReportUpdatedAt { get; set; } // When doctor filled the report

        public Status Status { get; set; }

        // Foreign Key to Doctor
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }

    }
    public enum Status
    {
        Critical=40,
        Normal=50,
        Routine=60
    }
}
