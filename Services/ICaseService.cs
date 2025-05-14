using HMS.Models;

namespace HMS.Services
{
    public interface ICaseService
    {
        Task<List<Case>> GetCasesByDoctorSpecializationAsync(string specialization);
        Task<List<Case>> GetCasesByDoctorIdAsync(string doctorId);
        Task<List<Case>> GetCasesByPatientIdAsync(string patientId);
        Task<Case?> GetCaseByIdAsync(int caseId);
        Task CreateCaseAsync(Case newCase);
        Task EditCaseAsync(Case updatedCase);
        Task AddDoctorCommentAsync(int caseId, string doctorId, string comment, string prescription, Status status);
    }
}
