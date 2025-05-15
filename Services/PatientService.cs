using HMS.Data;
using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Case>> GetCasesByPatientAsync(string patientId)
        {
            return await _context.Cases
                .Include(c => c.Doctor)
                .Where(c => c.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<Doctor> GetAssignedDoctorAsync(int caseId)
        {
            var caseEntry = await _context.Cases
                .Include(c => c.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(c => c.CaseId == caseId);

            return caseEntry?.Doctor;
        }

        public async Task<Patient> GetPatientProfileAsync(string userId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);
        }
    }
}
