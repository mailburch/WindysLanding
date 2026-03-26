using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Controllers
{
    public class VolunteerApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VolunteerApplications (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.VolunteerApplications.ToListAsync());
        }

        // GET: VolunteerApplications/Details/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerApplication = await _context.VolunteerApplications
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (volunteerApplication == null)
            {
                return NotFound();
            }

            return View(volunteerApplication);
        }

        // GET: VolunteerApplications/Create
        public IActionResult Create()
        {
            // If user is logged in, prefill their email (but not name)
            var model = new VolunteerApplication();

            if (User.Identity?.IsAuthenticated == true)
            {
                model.Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "";
            }

            return View(model);
        }

        // POST: VolunteerApplications/Create (REQUIRE LOGIN)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Name,Phone,VolunteerDescription,WaiverSigned")] VolunteerApplication volunteerApplication)
        {
            if (ModelState.IsValid)
            {
                // Override email with authenticated user's email (prevent spoofing)
                volunteerApplication.Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "";

                // Set server-side fields
                volunteerApplication.ApplicationDate = DateTime.Now;

                if (volunteerApplication.WaiverSigned)
                {
                    volunteerApplication.WaiverSignedDate ??= DateTime.Now;
                }
                else
                {
                    volunteerApplication.WaiverSignedDate = null;
                }

                _context.Add(volunteerApplication);
                await _context.SaveChangesAsync();

                ViewBag.Title = "Thank You for Volunteering!";
                ViewBag.Message = "Your volunteer application has been submitted successfully. Our team will review it and contact you soon.";

                return View("~/Views/Shared/ThankYou.cshtml");
            }

            return View(volunteerApplication);
        }

        // GET: VolunteerApplications/Edit/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerApplication = await _context.VolunteerApplications.FindAsync(id);
            if (volunteerApplication == null)
            {
                return NotFound();
            }
            return View(volunteerApplication);
        }

        // POST: VolunteerApplications/Edit/5 (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,Name,Email,Phone,VolunteerDescription,WaiverSigned,WaiverSignedDate")] VolunteerApplication volunteerApplication)
        {
            if (id != volunteerApplication.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerApplicationExists(volunteerApplication.ApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(volunteerApplication);
        }

        // GET: VolunteerApplications/Delete/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerApplication = await _context.VolunteerApplications
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (volunteerApplication == null)
            {
                return NotFound();
            }

            return View(volunteerApplication);
        }

        // POST: VolunteerApplications/Delete/5 (ADMIN ONLY)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerApplication = await _context.VolunteerApplications.FindAsync(id);
            if (volunteerApplication != null)
            {
                _context.VolunteerApplications.Remove(volunteerApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerApplicationExists(int id)
        {
            return _context.VolunteerApplications.Any(e => e.ApplicationId == id);
        }
    }
}
