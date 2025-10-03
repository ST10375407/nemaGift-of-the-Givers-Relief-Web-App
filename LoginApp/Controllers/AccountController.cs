using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApp.Models;
using LoginApp.Data;
using System.Security.Cryptography;
using System.Text;

namespace LoginApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(model);
            }

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user != null && VerifyPassword(login.Password, user.PasswordHash))
            {
                user.LastLoginDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // Save User info in session
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");

                TempData["SuccessMessage"] = $"Welcome back, {user.FirstName}!";

                // Redirect to Dashboard
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(login);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/Dashboard
        public IActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
                return RedirectToAction("Login");

            return View(); // Will show menu links to IncidentReports and ResourceDonations
        }

        // Hash password with SHA256
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // Verify password matches hash
        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
