using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class Case
    {
        public int CaseId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key to Patient
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        // Medical Report List (stored as text, not uploaded files)
        public List<string> SelectedConditions { get; set; } = new List<string>();

        // 🔴 These are for UI/file uploads — exclude from EF Core
        [NotMapped]
        public List<IFormFile> CaseDocuments { get; set; } = new List<IFormFile>();

        [NotMapped]
        public List<IFormFile> CaseImages { get; set; } = new List<IFormFile>();

        [NotMapped]
        public List<string> AvailableCondition { get; set; } = new List<string>
        {
            "Diabetes",
            "Hypertension",
            "Asthma",
            "Cancer",
            "Heart Disease",
            "Kidney Disease"
        };

        public string DoctorComments { get; set; }
        public string PrescribedMedicines { get; set; }
        public DateTime? ReportUpdatedAt { get; set; }

        public Status Status { get; set; }

        // Foreign Key to Doctor
        [ForeignKey("DoctorId")]

        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }

    public enum Status
    {
        Critical = 40,
        Normal = 50,
        Routine = 60
    }
}
