using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorController(IDoctorService doctorService, UserManager<ApplicationUser> userManager)
        {
            _doctorService = doctorService;
            _userManager = userManager;
        }

        // GET: Doctor List
        // Assuming only admins or you want to keep this? 
        // You may want to create a separate service for that or inject DbContext if needed
        // Leaving it for now without service refactor
        // If needed, you can inject ApplicationDbContext or create a service method for it

        // GET: View Cases for the logged-in Doctor
        public async Task<IActionResult> SpecializationCases()
        {
            var doctorUser = await _userManager.GetUserAsync(User);
            if (doctorUser == null) return Forbid();

            // You probably have ApplicationUserId as string, so get doctor id from DB
            // Here, to get doctorId as string, you can query ApplicationDbContext or inject a separate service for doctor info
            // Let's assume you will inject ApplicationDbContext or add a method in DoctorService to get doctorId by userId

            // For now, assume you get the doctorId as string from the user (or refactor this later)
            // For example: let's create a helper method in DoctorService: GetDoctorIdByUserIdAsync

            var doctorId = await _doctorService.GetDoctorIdByUserIdAsync(doctorUser.Id);
            if (string.IsNullOrEmpty(doctorId)) return NotFound();

            var cases = await _doctorService.GetCasesByDoctorAsync(doctorId);

            return View(cases);
        }

        // POST: Add Doctor Comments & Prescription to a Case
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctorNote(int caseId, string comment, string prescription)
        {
            var doctorUser = await _userManager.GetUserAsync(User);
            if (doctorUser == null) return Forbid();

            var doctorId = await _doctorService.GetDoctorIdByUserIdAsync(doctorUser.Id);
            if (string.IsNullOrEmpty(doctorId)) return NotFound();

            await _doctorService.UpdateDoctorCommentsAsync(caseId, comment, prescription);

            return RedirectToAction(nameof(SpecializationCases));
        }
    }
}
    