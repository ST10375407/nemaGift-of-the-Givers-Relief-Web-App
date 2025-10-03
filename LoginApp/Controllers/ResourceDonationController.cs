using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApp.Data;
using LoginApp.Models;

public class ResourceDonationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ResourceDonationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var donations = await _context.ResourceDonations
                                      .Include(d => d.User)
                                      .Where(d => d.UserID == userId)
                                      .ToListAsync();

        return View(donations);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ResourceDonation donation)
    {
        int? userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null)
            return RedirectToAction("Login", "Account");

        donation.UserID = userId.Value;
        donation.DateDonated = DateTime.Now;

        if (ModelState.IsValid)
        {
            _context.ResourceDonations.Add(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(donation);
    }
}
