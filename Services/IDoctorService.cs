using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Services
{
    public interface IDoctorService
    {
        Task<List<Case>> GetCasesByDoctorAsync(string doctorId);
        Task<List<Patient>> GetPatientsByDoctorAsync(string doctorId);
        Task UpdateDoctorCommentsAsync(int caseId, string comments, string medicines);
        Task<string> GetDoctorIdByUserIdAsync(string userId);  // NEW method
    }
}
