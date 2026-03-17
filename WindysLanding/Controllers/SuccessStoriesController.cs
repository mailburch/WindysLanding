using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Controllers
{
    public class SuccessStoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SuccessStoriesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var stories = await _context.SuccessStories
                .Include(s => s.Animal)
                .Include(s => s.Photos)
                .OrderByDescending(s => s.DatePublished)
                .ToListAsync();

            return View(stories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var successStory = await _context.SuccessStories
                .Include(s => s.Animal)
                .Include(s => s.Photos)
                .FirstOrDefaultAsync(m => m.StoryId == id);

            if (successStory == null)
            {
                return NotFound();
            }

            return View(successStory);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("StoryId,AnimalId,Title,Content,DatePublished")] SuccessStory successStory, List<IFormFile> photos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(successStory);
                await _context.SaveChangesAsync();

                // Upload photos if provided
                if (photos != null && photos.Count > 0)
                {
                    await UploadPhotos(successStory.StoryId, photos);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", successStory.AnimalId);
            return View(successStory);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var successStory = await _context.SuccessStories
                .Include(s => s.Photos)
                .FirstOrDefaultAsync(s => s.StoryId == id);

            if (successStory == null)
            {
                return NotFound();
            }

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", successStory.AnimalId);
            return View(successStory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("StoryId,AnimalId,Title,Content,DatePublished")] SuccessStory successStory, List<IFormFile> photos)
        {
            if (id != successStory.StoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(successStory);
                    await _context.SaveChangesAsync();

                    // Upload new photos if provided
                    if (photos != null && photos.Count > 0)
                    {
                        var hasFiles = photos.Any(p => p != null && p.Length > 0);
                        if (hasFiles)
                        {
                            await UploadPhotos(successStory.StoryId, photos);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SuccessStories.Any(e => e.StoryId == successStory.StoryId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", successStory.AnimalId);
            return View(successStory);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var successStory = await _context.SuccessStories
                .Include(s => s.Animal)
                .Include(s => s.Photos)
                .FirstOrDefaultAsync(m => m.StoryId == id);

            if (successStory == null)
            {
                return NotFound();
            }

            return View(successStory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var successStory = await _context.SuccessStories
                .Include(s => s.Photos)
                .FirstOrDefaultAsync(s => s.StoryId == id);

            if (successStory != null)
            {
                // Delete photos from filesystem
                if (successStory.Photos != null && successStory.Photos.Any())
                {
                    foreach (var photo in successStory.Photos)
                    {
                        DeletePhotoFile(photo.ImgUrl);
                    }
                }

                _context.SuccessStories.Remove(successStory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Delete individual photo (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo != null)
            {
                var storyId = photo.SuccessStoryId;
                DeletePhotoFile(photo.ImgUrl);
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Edit), new { id = storyId });
            }

            return NotFound();
        }

        // Upload photos helper
        private async Task UploadPhotos(int storyId, List<IFormFile> photos)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "stories");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            int uploadedCount = 0;
            foreach (var photo in photos)
            {
                if (photo == null || photo.Length == 0)
                    continue;

                if (photo.Length > 10 * 1024 * 1024)
                    continue;

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    continue;

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }

                _context.Photos.Add(new Photo
                {
                    SuccessStoryId = storyId,
                    ImgUrl = "/images/stories/" + uniqueFileName,
                    Caption = ""
                });

                uploadedCount++;
            }

            if (uploadedCount > 0)
            {
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