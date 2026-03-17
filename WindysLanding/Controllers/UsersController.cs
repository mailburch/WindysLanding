using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.FullName)
                .ToListAsync();

            // Create a list to hold user info with roles
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    NewsletterOptIn = user.NewsletterOptIn,
                    IsAdmin = roles.Contains("Admin")
                });
            }

            return View(userList);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                NewsletterOptIn = user.NewsletterOptIn,
                IsAdmin = roles.Contains("Admin")
            };

            return View(viewModel);
        }

        // POST: Users/ToggleNewsletter/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleNewsletter(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.NewsletterOptIn = !user.NewsletterOptIn;
            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = $"Newsletter preference updated for {user.FullName}";
            return RedirectToAction(nameof(Index));
        }

        // POST: Users/ToggleAdmin/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                TempData["SuccessMessage"] = $"Removed admin role from {user.FullName}";
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["SuccessMessage"] = $"Added admin role to {user.FullName}";
            }

            return RedirectToAction(nameof(Index));
        }
    }

    // ViewModel for displaying user information
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool NewsletterOptIn { get; set; }
        public bool IsAdmin { get; set; }
    }
}