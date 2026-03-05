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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhotoId,AnimalId,SponsorCompanyId,NewsletterId,EventId,ImgUrl,Caption")] Photo photo, IFormFile ImageFile)
        {
            // Remove ImgUrl validation if file is being uploaded
            if (ImageFile != null && ImageFile.Length > 0)
            {
                ModelState.Remove("ImgUrl");
            }

            // Handle file upload if provided
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Validate file size (10MB max)
                if (ImageFile.Length > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot exceed 10MB");
                    ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", photo.AnimalId);
                    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", photo.EventId);
                    ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title", photo.NewsletterId);
                    ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name", photo.SponsorCompanyId);
                    return View(photo);
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (JPG, PNG, GIF, WEBP) are allowed");
                    ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", photo.AnimalId);
                    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", photo.EventId);
                    ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title", photo.NewsletterId);
                    ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name", photo.SponsorCompanyId);
                    return View(photo);
                }

                // Generate unique filename
                var fileName = Guid.NewGuid().ToString() + extension;

                // Determine subfolder based on what the photo belongs to
                string subfolder = "general";
                if (photo.AnimalId.HasValue) subfolder = "animals";
                else if (photo.SponsorCompanyId.HasValue) subfolder = "sponsors";
                else if (photo.EventId.HasValue) subfolder = "events";
                else if (photo.NewsletterId.HasValue) subfolder = "newsletters";

                // Create directory if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", subfolder);
                Directory.CreateDirectory(uploadsFolder);

                // Save file
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Set the ImgUrl to the saved file path
                photo.ImgUrl = $"/images/{subfolder}/{fileName}";
            }

            // If no file uploaded and no URL provided, return error
            if (string.IsNullOrWhiteSpace(photo.ImgUrl))
            {
                ModelState.AddModelError("ImageFile", "Please upload an image or provide an image URL");
                ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", photo.AnimalId);
                ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", photo.EventId);
                ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title", photo.NewsletterId);
                ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name", photo.SponsorCompanyId);
                return View(photo);
            }

            if (ModelState.IsValid)
            {
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", photo.AnimalId);
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", photo.EventId);
            ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title", photo.NewsletterId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoId,AnimalId,SponsorCompanyId,NewsletterId,EventId,ImgUrl,Caption")] Photo photo, IFormFile ImageFile)
        {
            if (id != photo.PhotoId)
            {
                return NotFound();
            }

            // Get the existing photo from database to preserve the old ImgUrl if needed
            var existingPhoto = await _context.Photos.AsNoTracking().FirstOrDefaultAsync(p => p.PhotoId == id);
            if (existingPhoto == null)
            {
                return NotFound();
            }

            // Handle file upload if provided
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Remove ImgUrl validation if file is being uploaded
                ModelState.Remove("ImgUrl");

                // Validate file size (10MB max)
                if (ImageFile.Length > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot exceed 10MB");
                    ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", photo.AnimalId);
                    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", photo.EventId);
                    ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title", photo.NewsletterId);
                    ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name", photo.SponsorCompanyId);
                    return View(photo);
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (JPG, PNG, GIF, WEBP) are allowed");
                    ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", photo.AnimalId);
                    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", photo.EventId);
                    ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title", photo.NewsletterId);
                    ViewData["SponsorCompanyId"] = new SelectList(_context.SponsorCompanies, "SponsorCompanyId", "Name", photo.SponsorCompanyId);
                    return View(photo);
                }

                // Delete old file if it exists and is a local file
                if (!string.IsNullOrEmpty(existingPhoto.ImgUrl) && existingPhoto.ImgUrl.StartsWith("/images/"))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPhoto.ImgUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Generate unique filename
                var fileName = Guid.NewGuid().ToString() + extension;

                // Determine subfolder based on what the photo belongs to
                string subfolder = "general";
                if (photo.AnimalId.HasValue) subfolder = "animals";
                else if (photo.SponsorCompanyId.HasValue) subfolder = "sponsors";
                else if (photo.EventId.HasValue) subfolder = "events";
                else if (photo.NewsletterId.HasValue) subfolder = "newsletters";

                // Create directory if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", subfolder);
                Directory.CreateDirectory(uploadsFolder);

                // Save file
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Set the ImgUrl to the saved file path
                photo.ImgUrl = $"/images/{subfolder}/{fileName}";
            }
            else if (string.IsNullOrWhiteSpace(photo.ImgUrl))
            {
                // No new file uploaded and URL is empty - keep the existing URL
                photo.ImgUrl = existingPhoto.ImgUrl;
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

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", photo.AnimalId);
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", photo.EventId);
            ViewData["NewsletterId"] = new SelectList(_context.Newsletters, "NewsletterId", "Title", photo.NewsletterId);
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