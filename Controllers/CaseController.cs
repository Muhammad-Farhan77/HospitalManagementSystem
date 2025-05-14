using HMS.Data;
using HMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Controllers
{
    public class CaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CaseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // List all cases
        public async Task<IActionResult> Index()
        {
            var cases = await _context.Cases
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .ToListAsync();

            return View(cases);
        }

        // GET: Create new case (only Patient should do this)
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || currentUser.Role != "Patient")
            {
                return Forbid(); // Only patients can create cases
            }

            var model = new Case
            {
                PatientId = currentUser.Id,
                AvailableCondition = new Case().AvailableCondition
            };

            return View(model);
        }

        // POST: Create new case
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Case model)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                model.PatientId = currentUser.Id;
                model.CreatedAt = DateTime.UtcNow;
                model.Status = Status.Routine; // Default

                // You can handle file saving logic here if needed
                // Example: Save to disk/cloud, then store file paths

                _context.Cases.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.AvailableCondition = new Case().AvailableCondition;
            return View(model);
        }

        // GET: Edit case (Doctor only)
        public async Task<IActionResult> Edit(int id)
        {
            var caseData = await _context.Cases
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(c => c.CaseId == id);

            var currentUser = await _userManager.GetUserAsync(User);
            if (caseData == null || currentUser?.Role != "Doctor")
            {
                return Forbid(); // Only doctors can edit
            }

            return View(caseData);
        }

        // POST: Edit case by doctor (add comments, prescriptions, etc.)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Case updated)
        {
            var caseData = await _context.Cases.FindAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);

            if (caseData == null || currentUser?.Role != "Doctor")
            {
                return Forbid();
            }

            caseData.DoctorId = currentUser.Id;
            caseData.DoctorComments = updated.DoctorComments;
            caseData.PrescribedMedicines = updated.PrescribedMedicines;
            caseData.Status = updated.Status;
            caseData.ReportUpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var caseDetails = await _context.Cases
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(c => c.CaseId == id);

            if (caseDetails == null) return NotFound();

            return View(caseDetails);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var caseToDelete = await _context.Cases
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(c => c.CaseId == id);

            if (caseToDelete == null) return NotFound();

            return View(caseToDelete);
        }

        // POST: Confirm Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseData = await _context.Cases.FindAsync(id);
            if (caseData != null)
            {
                _context.Cases.Remove(caseData);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
