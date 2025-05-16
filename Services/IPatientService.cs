using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Services
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);
        Task<Patient> GetPatientDetailsAsync(int id); // with related data (cases, user)
        Task CreatePatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(int id);

        Task<List<Case>> GetCasesByPatientAsync(string patientId);
        Task<Doctor> GetAssignedDoctorAsync(int caseId);
        Task<Patient> GetPatientProfileAsync(string userId);
    }
}
