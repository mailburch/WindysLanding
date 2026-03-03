using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: VolunteerApplications
        public async Task<IActionResult> Index()
        {
            return View(await _context.VolunteerApplications.ToListAsync());
        }

        // GET: VolunteerApplications/Details/5
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
            return View();
        }

        // POST: VolunteerApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplicationId,Name,Email,Phone,VolunteerDescription,ApplicationDate,WaiverSigned,WaiverSignedDate")] VolunteerApplication volunteerApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteerApplication);
        }

        // GET: VolunteerApplications/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,Name,Email,Phone,VolunteerDescription,ApplicationDate,WaiverSigned,WaiverSignedDate")] VolunteerApplication volunteerApplication)
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

        // GET: VolunteerApplications/Delete/5
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

        // POST: VolunteerApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
