using HMS.Models;

namespace HMS.Services
{
    public interface IPatientService
    {
        Task<List<Case>> GetCasesByPatientAsync(string patientId);
        Task<Doctor> GetAssignedDoctorAsync(int caseId);
        Task<Patient> GetPatientProfileAsync(string userId);
    }
}