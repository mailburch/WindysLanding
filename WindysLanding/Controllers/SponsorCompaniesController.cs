using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Controllers
{
    public class SponsorCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SponsorCompaniesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: SponsorCompanies (Admin - manage sponsors)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var sponsors = await _context.SponsorCompanies
                .Include(s => s.Photos)
                .ToListAsync();
            return View(sponsors);
        }

        // GET: SponsorCompanies/Wall (Public - wall of sponsors)
        public async Task<IActionResult> Wall()
        {
            var sponsors = await _context.SponsorCompanies
                .Include(s => s.Photos)
                .ToListAsync();
            return View(sponsors);
        }

        // GET: SponsorCompanies/Details/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sponsorCompany = await _context.SponsorCompanies
                .Include(s => s.Photos)
                .FirstOrDefaultAsync(m => m.SponsorCompanyId == id);

            if (sponsorCompany == null) return NotFound();

            return View(sponsorCompany);
        }

        // GET: SponsorCompanies/Create (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SponsorCompanies/Create (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("SponsorCompanyId,Name,Description")] SponsorCompany sponsorCompany, IFormFile logo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sponsorCompany);
                await _context.SaveChangesAsync();

                // Upload logo if provided
                if (logo != null && logo.Length > 0)
                {
                    await UploadLogo(sponsorCompany.SponsorCompanyId, logo);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(sponsorCompany);
        }

        // GET: SponsorCompanies/Edit/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sponsorCompany = await _context.SponsorCompanies
                .Include(s => s.Photos)
                .FirstOrDefaultAsync(s => s.SponsorCompanyId == id);

            if (sponsorCompany == null) return NotFound();

            return View(sponsorCompany);
        }

        // POST: SponsorCompanies/Edit/5 (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("SponsorCompanyId,Name,Description")] SponsorCompany sponsorCompany, IFormFile logo)
        {
            if (id != sponsorCompany.SponsorCompanyId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponsorCompany);
                    await _context.SaveChangesAsync();

                    // Upload new logo if provided
                    if (logo != null && logo.Length > 0)
                    {
                        // Delete old logo first
                        var existingPhotos = await _context.Photos
                            .Where(p => p.SponsorCompanyId == id)
                            .ToListAsync();

                        foreach (var photo in existingPhotos)
                        {
                            DeletePhotoFile(photo.ImgUrl);
                            _context.Photos.Remove(photo);
                        }
                        await _context.SaveChangesAsync();

                        // Upload new logo
                        await UploadLogo(sponsorCompany.SponsorCompanyId, logo);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorCompanyExists(sponsorCompany.SponsorCompanyId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sponsorCompany);
        }

        // GET: SponsorCompanies/Delete/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sponsorCompany = await _context.SponsorCompanies
                .Include(s => s.Photos)
                .FirstOrDefaultAsync(m => m.SponsorCompanyId == id);

            if (sponsorCompany == null) return NotFound();

            return View(sponsorCompany);
        }

        // POST: SponsorCompanies/Delete/5 (ADMIN ONLY)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsorCompany = await _context.SponsorCompanies
                .Include(s => s.Photos)
                .FirstOrDefaultAsync(s => s.SponsorCompanyId == id);

            if (sponsorCompany != null)
            {
                // Delete logo files
                if (sponsorCompany.Photos != null && sponsorCompany.Photos.Any())
                {
                    foreach (var photo in sponsorCompany.Photos)
                    {
                        DeletePhotoFile(photo.ImgUrl);
                    }
                }

                _context.SponsorCompanies.Remove(sponsorCompany);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: SponsorCompanies/DeleteLogo/5 (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLogo(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo != null)
            {
                var sponsorId = photo.SponsorCompanyId;
                DeletePhotoFile(photo.ImgUrl);
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Edit), new { id = sponsorId });
            }

            return NotFound();
        }

        private bool SponsorCompanyExists(int id)
        {
            return _context.SponsorCompanies.Any(e => e.SponsorCompanyId == id);
        }

        // Upload logo helper
        private async Task UploadLogo(int sponsorId, IFormFile logo)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "sponsors");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            if (logo.Length > 0)
            {
                // Validate file size (5MB max for logos)
                if (logo.Length > 5 * 1024 * 1024)
                {
                    return;
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
                var extension = Path.GetExtension(logo.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    return;
                }

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await logo.CopyToAsync(fileStream);
                }

                _context.Photos.Add(new Photo
                {
                    SponsorCompanyId = sponsorId,
                    ImgUrl = "/images/sponsors/" + uniqueFileName,
                    Caption = ""
                });

                await _context.SaveChangesAsync();
            }
        }

        // Delete photo file helper
        private void DeletePhotoFile(string imgUrl)
        {
            if (!string.IsNullOrEmpty(imgUrl))
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, imgUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}