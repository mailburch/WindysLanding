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
    public class SponsorCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SponsorCompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SponsorCompanies
        public async Task<IActionResult> Index()
        {
            return View(await _context.SponsorCompanies.ToListAsync());
        }

        // GET: SponsorCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorCompany = await _context.SponsorCompanies
                .FirstOrDefaultAsync(m => m.SponsorCompanyId == id);
            if (sponsorCompany == null)
            {
                return NotFound();
            }

            return View(sponsorCompany);
        }

        // GET: SponsorCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SponsorCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SponsorCompanyId,Name,Description")] SponsorCompany sponsorCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sponsorCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sponsorCompany);
        }

        // GET: SponsorCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorCompany = await _context.SponsorCompanies.FindAsync(id);
            if (sponsorCompany == null)
            {
                return NotFound();
            }
            return View(sponsorCompany);
        }

        // POST: SponsorCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SponsorCompanyId,Name,Description")] SponsorCompany sponsorCompany)
        {
            if (id != sponsorCompany.SponsorCompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponsorCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorCompanyExists(sponsorCompany.SponsorCompanyId))
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
            return View(sponsorCompany);
        }

        // GET: SponsorCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorCompany = await _context.SponsorCompanies
                .FirstOrDefaultAsync(m => m.SponsorCompanyId == id);
            if (sponsorCompany == null)
            {
                return NotFound();
            }

            return View(sponsorCompany);
        }

        // POST: SponsorCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsorCompany = await _context.SponsorCompanies.FindAsync(id);
            if (sponsorCompany != null)
            {
                _context.SponsorCompanies.Remove(sponsorCompany);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorCompanyExists(int id)
        {
            return _context.SponsorCompanies.Any(e => e.SponsorCompanyId == id);
        }
    }
}
