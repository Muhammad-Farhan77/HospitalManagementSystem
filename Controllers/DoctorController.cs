using HMS.Data;
using HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Doctor List
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .ToListAsync();

            return View(doctors);
        }

        // GET: Doctor/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DoctorId == id);

            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // GET: Doctor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                model.ApplicationUserId = currentUser?.Id;

                _context.Doctors.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Doctor/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor updated)
        {
            if (id != updated.DoctorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(updated);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Doctors.Any(e => e.DoctorId == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(updated);
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DoctorId == id);

            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // POST: Doctor/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: View Cases Based on Specialization
        public async Task<IActionResult> SpecializationCases()
        {
            var doctorUser = await _userManager.GetUserAsync(User);
            if (doctorUser == null || doctorUser.Role != "Doctor") return Forbid();

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == doctorUser.Id);
            if (doctor == null) return NotFound();

            // Get cases where DoctorId matches this doctor
            var cases = await _context.Cases
                .Where(c => c.DoctorId == doctor.DoctorId)
                .Include(c => c.Patient)
                .ToListAsync();

            return View(cases);
        }

        // POST: Add a Comment or Prescription to a Case
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctorNote(int id, string comment, string prescription, Status status)
        {
            var doctorUser = await _userManager.GetUserAsync(User);
            if (doctorUser == null || doctorUser.Role != "Doctor") return Forbid();

            var caseToUpdate = await _context.Cases.FindAsync(id);
            if (caseToUpdate != null)
            {
                caseToUpdate.DoctorComments = comment;
                caseToUpdate.PrescribedMedicines = prescription;
                caseToUpdate.Status = status;
                caseToUpdate.ReportUpdatedAt = DateTime.UtcNow;

                _context.Update(caseToUpdate);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(SpecializationCases));
        }
    }
}
