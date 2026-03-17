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
            // TODO (Google Auth milestone):
            // - If User is authenticated, prefill Name/Email from claims and pass a prefilled model to the View.
            // - When auth is enforced, add [Authorize] to Create GET + POST (or controller).

            return View();
        }

        // POST: VolunteerApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: VolunteerApplications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Phone,VolunteerDescription,WaiverSigned")] VolunteerApplication volunteerApplication)
        {
            if (ModelState.IsValid)
            {
                // These are not on the public form — set them server-side.
                volunteerApplication.ApplicationDate = DateTime.Now;

                if (volunteerApplication.WaiverSigned)
                {
                    volunteerApplication.WaiverSignedDate ??= DateTime.Now;
                }
                else
                {
                    volunteerApplication.WaiverSignedDate = null;
                }

                // TODO (Google Auth milestone):
                // - Overwrite volunteerApplication.Name and volunteerApplication.Email from authenticated user claims
                //   before saving (prevents spoofing).

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

        // POST: VolunteerApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    // Prevent the application date from being changed via Edit form:
                    // Pull original from DB and keep it.
                    var existing = await _context.VolunteerApplications.AsNoTracking()
                        .FirstOrDefaultAsync(v => v.ApplicationId == id);

                    if (existing == null)
                        return NotFound();

                    volunteerApplication.ApplicationDate = existing.ApplicationDate;

                    // Keeps waiver date consistent
                    if (volunteerApplication.WaiverSigned)
                    {
                        volunteerApplication.WaiverSignedDate ??= DateTime.Now;
                    }
                    else
                    {
                        volunteerApplication.WaiverSignedDate = null;
                    }

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
