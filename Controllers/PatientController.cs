using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientController(IPatientService patientService, UserManager<ApplicationUser> userManager)
        {
            _patientService = patientService;
            _userManager = userManager;
        }

        // GET: List all patients
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return View(patients);
        }

        // GET: Patient details including cases
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientService.GetPatientDetailsAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // GET: Create patient view
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create new patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient model)
        {
            if (ModelState.IsValid)
            {
                await _patientService.CreatePatientAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Edit patient view
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Edit patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient model)
        {
            if (id != model.PatientId) return NotFound();

            if (ModelState.IsValid)
            {
                await _patientService.UpdatePatientAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Delete confirmation view
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Confirm delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _patientService.DeletePatientAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Additional: Get cases for a patient by user id
        public async Task<IActionResult> Cases(string userId)
        {
            var cases = await _patientService.GetCasesByPatientAsync(userId);
            return View(cases);
        }

        // Additional: Get assigned doctor for a case
        public async Task<IActionResult> AssignedDoctor(int caseId)
        {
            var doctor = await _patientService.GetAssignedDoctorAsync(caseId);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // Additional: Get patient profile by user id
        public async Task<IActionResult> Profile(string userId)
        {
            var patient = await _patientService.GetPatientProfileAsync(userId);
            if (patient == null) return NotFound();
            return View(patient);
        }
        // GET: Patient Dashboard
public async Task<IActionResult> Dashboard()
{
    var user = await _userManager.GetUserAsync(User);
    var patient = await _patientService.GetPatientProfileAsync(user.Id);

    var cases = await _patientService.GetCasesByPatientAsync(user.Id);
    var totalReports = cases.Count;
    var underReview = cases.Count(c => c.Status == Status.Critical || c.Status == Status.Routine);

    ViewBag.TotalCases = cases.Count;
    ViewBag.TotalReports = totalReports;
    ViewBag.UnderReview = underReview;

    return View("Dashboard", patient); // 👈 make sure it points to Views/Patient/Dashboard.cshtml
}

    }
}
