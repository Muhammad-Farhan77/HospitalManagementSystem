using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class CaseController : Controller
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        // GET: Cases assigned to current doctor
        public async Task<IActionResult> Index()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cases = await _caseService.GetCasesByDoctorIdAsync(doctorId);
            return View(cases);
        }

        // GET: Case Details
        public async Task<IActionResult> Details(int id)
        {
            var caseDetails = await _caseService.GetCaseByIdAsync(id);
            if (caseDetails == null)
                return NotFound();

            return View(caseDetails);
        }

        // GET: Create Case (for admin or general purpose if needed)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Case
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Case model)
        {
            if (ModelState.IsValid)
            {
                await _caseService.CreateCaseAsync(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Edit Case
        public async Task<IActionResult> Edit(int id)
        {
            var caseToEdit = await _caseService.GetCaseByIdAsync(id);
            if (caseToEdit == null)
                return NotFound();

            return View(caseToEdit);
        }

        // POST: Edit Case
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Case updatedCase)
        {
            if (ModelState.IsValid)
            {
                await _caseService.EditCaseAsync(updatedCase);
                return RedirectToAction(nameof(Index));
            }

            return View(updatedCase);
        }

        // POST: Add Doctor's Note
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctorNote(int id, string comment, string prescription, Status status)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _caseService.AddDoctorCommentAsync(id, doctorId, comment, prescription, status);
            return RedirectToAction(nameof(Index));
        }
    }
}
