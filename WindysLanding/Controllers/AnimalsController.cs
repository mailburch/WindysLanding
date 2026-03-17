using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AnimalsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Animals (ADMIN ONLY - management view)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchString, string animalType, bool? adoptionStatus)
        {
            var animals = _context.Animals.Include(a => a.Photos).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                animals = animals.Where(a => a.Name.Contains(searchString) ||
                                           (a.Breed != null && a.Breed.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(animalType))
            {
                animals = animals.Where(a => a.AnimalType == animalType);
            }

            if (adoptionStatus.HasValue)
            {
                animals = animals.Where(a => a.AdoptionStatus == adoptionStatus.Value);
            }

            ViewBag.SearchString = searchString;
            ViewBag.AnimalType = animalType;
            ViewBag.AdoptionStatus = adoptionStatus;
            ViewBag.AnimalTypes = await _context.Animals.Select(a => a.AnimalType).Distinct().ToListAsync();

            return View(await animals.ToListAsync());
        }

        // GET: Animals/Adoption (Public view - available animals only)
        public async Task<IActionResult> Adoption(string searchString, string animalType)
        {
            var animals = _context.Animals
                .Include(a => a.Photos)
                .Where(a => a.AdoptionStatus == false)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                animals = animals.Where(a => a.Name.Contains(searchString) ||
                                           (a.Breed != null && a.Breed.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(animalType))
            {
                animals = animals.Where(a => a.AnimalType == animalType);
            }

            ViewBag.SearchString = searchString;
            ViewBag.AnimalType = animalType;
            ViewBag.AnimalTypes = await _context.Animals
                .Where(a => a.AdoptionStatus == false)
                .Select(a => a.AnimalType)
                .Distinct()
                .ToListAsync();

            return View(await animals.ToListAsync());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animals
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(m => m.AnimalId == id);

            if (animal == null) return NotFound();

            return View(animal);
        }

        // GET: Animals/Create (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Animals/Create (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("AnimalId,Name,AnimalType,Age,Size,Description,Gender,Breed,Color,Experience,AdoptionStatus,SponsorshipAvailable,SponsorshipUrl")] Animal animal, List<IFormFile> photos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();

                if (photos != null && photos.Count > 0)
                {
                    await UploadPhotos(animal.AnimalId, photos);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(animal);
        }

        // GET: Animals/Edit/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animals
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(a => a.AnimalId == id);

            if (animal == null) return NotFound();

            return View(animal);
        }

        // POST: Animals/Edit/5 (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("AnimalId,Name,AnimalType,Age,Size,Description,Gender,Breed,Color,Experience,AdoptionStatus,SponsorshipAvailable,SponsorshipUrl")] Animal animal, List<IFormFile> photos)
        {
            if (id != animal.AnimalId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Save animal info first
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.AnimalId))
                        return NotFound();
                    else
                        throw;
                }

                // Upload photos AFTER animal is saved
                if (photos != null && photos.Count > 0)
                {
                    // Check if any files were actually selected
                    var hasFiles = photos.Any(p => p != null && p.Length > 0);
                    if (hasFiles)
                    {
                        await UploadPhotos(animal.AnimalId, photos);
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(animal);
        }

        // GET: Animals/Delete/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animals
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(m => m.AnimalId == id);

            if (animal == null) return NotFound();

            return View(animal);
        }

        // POST: Animals/Delete/5 (ADMIN ONLY)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(a => a.AnimalId == id);

            if (animal != null)
            {
                if (animal.Photos != null && animal.Photos.Any())
                {
                    foreach (var photo in animal.Photos)
                    {
                        DeletePhotoFile(photo.ImgUrl);
                    }
                }

                _context.Animals.Remove(animal);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Animals/DeletePhoto/5 (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo != null)
            {
                var animalId = photo.AnimalId;
                DeletePhotoFile(photo.ImgUrl);
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Edit), new { id = animalId });
            }

            return NotFound();
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.AnimalId == id);
        }

        private async Task UploadPhotos(int animalId, List<IFormFile> photos)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "animals");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            int uploadedCount = 0;
            foreach (var photo in photos)
            {
                if (photo == null || photo.Length == 0)
                    continue;

                // Validate file size (10MB max)
                if (photo.Length > 10 * 1024 * 1024)
                {
                    continue; // Skip files > 10MB
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    continue; // Skip invalid types
                }

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }

                _context.Photos.Add(new Photo
                {
                    AnimalId = animalId,
                    ImgUrl = "/images/animals/" + uniqueFileName,
                    Caption = ""
                });

                uploadedCount++;
            }

            if (uploadedCount > 0)
            {
                await _context.SaveChangesAsync();
            }
        }

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