using HMS.Data;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients.Include(p => p.User).ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task<Patient> GetPatientDetailsAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Cases)
                .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task CreatePatientAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await GetPatientByIdAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Case>> GetCasesByPatientAsync(string patientId)
        {
            if (!int.TryParse(patientId, out var patientIntId))
                return new List<Case>();

            return await _context.Cases
                .Include(c => c.Doctor)
                .Where(c => c.PatientId == patientIntId)
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
