using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApp.Data;
using LoginApp.Models;

namespace LoginApp.Controllers
{
    public class IncidentReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /IncidentReports
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var reports = await _context.IncidentReports
                .Where(r => r.UserID == userId)
                .ToListAsync();

            return View(reports);
        }

        // GET: /IncidentReports/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: /IncidentReports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidentReport model)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                model.UserID = userId.Value;
                model.DateReported = DateTime.Now;

                _context.IncidentReports.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Incident reported successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
