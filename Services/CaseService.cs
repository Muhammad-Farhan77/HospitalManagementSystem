using HMS.Data;
using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class CaseService : ICaseService
    {
        private readonly ApplicationDbContext _context;

        public CaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get cases where SelectedConditions contain doctor's specialization
        public async Task<List<Case>> GetCasesByDoctorSpecializationAsync(string specialization)
        {
            return await _context.Cases
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .Where(c => c.SelectedConditions.Contains(specialization))
                .ToListAsync();
        }

        public async Task<List<Case>> GetCasesByDoctorIdAsync(string doctorId)
        {
            if (!int.TryParse(doctorId, out var doctorIntId))
                return new List<Case>(); // return empty list if parsing fails

            return await _context.Cases
                .Include(c => c.Patient)
                .Where(c => c.DoctorId == doctorIntId)
                .ToListAsync();
        }

        public async Task<List<Case>> GetCasesByPatientIdAsync(string patientId)
        {
            if (!int.TryParse(patientId, out var patientIntId))
                return new List<Case>(); // return empty list if parsing fails

            return await _context.Cases
                .Include(c => c.Doctor)
                .Where(c => c.PatientId == patientIntId)
                .ToListAsync();
        }

        public async Task<Case?> GetCaseByIdAsync(int caseId)
        {
            return await _context.Cases
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(c => c.CaseId == caseId);
        }

        public async Task CreateCaseAsync(Case newCase)
        {
            _context.Cases.Add(newCase);
            await _context.SaveChangesAsync();
        }

        public async Task EditCaseAsync(Case updatedCase)
        {
            _context.Cases.Update(updatedCase);
            await _context.SaveChangesAsync();
        }

        // Add doctor's comments and prescription to a case
        public async Task AddDoctorCommentAsync(int caseId, string doctorId, string comment, string prescription, Status status)
        {
            var caseToUpdate = await _context.Cases.FindAsync(caseId);
            if (caseToUpdate != null)
            {
                if (int.TryParse(doctorId, out var doctorIntId))
                {
                    caseToUpdate.DoctorId = doctorIntId;
                }

                caseToUpdate.DoctorComments = comment;
                caseToUpdate.PrescribedMedicines = prescription;
                caseToUpdate.Status = status;
                caseToUpdate.ReportUpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }
    }
}
