using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApp.Data;
using LoginApp.Models;

namespace LoginApp.Controllers
{
    public class VolunteerTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /VolunteerTasks
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var tasks = await _context.VolunteerTasks
                                      .Where(t => t.UserID == userId)
                                      .ToListAsync();

            return View(tasks);
        }

        // GET: /VolunteerTasks/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: /VolunteerTasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolunteerTask task)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            task.UserID = userId.Value;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                TempData["ErrorMessage"] = string.Join(" | ", errors);
                return View(task);
            }

            try
            {
                _context.VolunteerTasks.Add(task);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Task added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to save task: " + ex.Message;
                return View(task);
            }
        }
    }
}
