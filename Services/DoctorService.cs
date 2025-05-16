using HMS.Data;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Case>> GetCasesByDoctorAsync(string doctorId)
        {
            if (!int.TryParse(doctorId, out var doctorIntId))
                return new List<Case>();

            return await _context.Cases
                .Include(c => c.Patient)
                .Where(c => c.DoctorId == doctorIntId)
                .ToListAsync();
        }

        public async Task<List<Patient>> GetPatientsByDoctorAsync(string doctorId)
        {
            if (!int.TryParse(doctorId, out var doctorIntId))
                return new List<Patient>();

            return await _context.Cases
                .Include(c => c.Patient)
                .ThenInclude(p => p.User)
                .Where(c => c.DoctorId == doctorIntId)
                .Select(c => c.Patient)
                .Distinct()
                .ToListAsync();
        }

        public async Task UpdateDoctorCommentsAsync(int caseId, string comments, string medicines)
        {
            var existingCase = await _context.Cases.FindAsync(caseId);
            if (existingCase != null)
            {
                existingCase.DoctorComments = comments;
                existingCase.PrescribedMedicines = medicines;
                existingCase.ReportUpdatedAt = System.DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        // NEW method to get DoctorId string by ApplicationUserId string
        public async Task<string> GetDoctorIdByUserIdAsync(string userId)
        {
            var doctor = await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.ApplicationUserId == userId);

            return doctor?.DoctorId.ToString();  // Convert int to string to match your interface
        }
    }
}
