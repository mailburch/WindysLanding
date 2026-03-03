using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Photos.Include(p => p.Animal).Include(p => p.Event).Include(p => p.Newsletter).Include(p => p.SponsorCompany);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Animal)
                .Include(p => p.Event)
                .Include(p => p.Newsletter)
                .Include(p => p.SponsorCompany)
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name");
            ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name");
            ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title");
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhotoId,AnimalId,SponsorCompanyId,NewsletterId,EventId,ImgUrl,Caption")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalType", photo.AnimalId);
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Description", photo.EventId);
            ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "NewsletterUrl", photo.NewsletterId);
            ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name", photo.SponsorCompanyId);
            return View(photo);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", photo.AnimalId);
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", photo.EventId);
            ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title", photo.NewsletterId);
            ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name", photo.SponsorCompanyId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoId,AnimalId,SponsorCompanyId,NewsletterId,EventId,ImgUrl,Caption")] Photo photo)
        {
            if (id != photo.PhotoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.PhotoId))
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
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalType", photo.AnimalId);
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Description", photo.EventId);
            ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "NewsletterUrl", photo.NewsletterId);
            ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name", photo.SponsorCompanyId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Animal)
                .Include(p => p.Event)
                .Include(p => p.Newsletter)
                .Include(p => p.SponsorCompany)
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.PhotoId == id);
        }
    }
}
