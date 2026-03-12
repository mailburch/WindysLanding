using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Controllers
{
    public class SuccessStoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuccessStoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stories = await _context.SuccessStories
                .Include(s => s.Animal)
                .Include(s => s.Photo)
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
                .Include(s => s.Photo)
                .FirstOrDefaultAsync(m => m.StoryId == id);

            if (successStory == null)
            {
                return NotFound();
            }

            return View(successStory);
        }

        public IActionResult Create()
        {
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalType");
            ViewData["PhotoId"] = new SelectList(_context.Photos, "PhotoId", "Caption");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoryId,AnimalId,PhotoId,Title,Content,DatePublished")] SuccessStory successStory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(successStory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalType", successStory.AnimalId);
            ViewData["PhotoId"] = new SelectList(_context.Photos, "PhotoId", "Caption", successStory.PhotoId);
            return View(successStory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var successStory = await _context.SuccessStories.FindAsync(id);
            if (successStory == null)
            {
                return NotFound();
            }

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalType", successStory.AnimalId);
            ViewData["PhotoId"] = new SelectList(_context.Photos, "PhotoId", "Caption", successStory.PhotoId);
            return View(successStory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoryId,AnimalId,PhotoId,Title,Content,DatePublished")] SuccessStory successStory)
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

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalType", successStory.AnimalId);
            ViewData["PhotoId"] = new SelectList(_context.Photos, "PhotoId", "Caption", successStory.PhotoId);
            return View(successStory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var successStory = await _context.SuccessStories
                .Include(s => s.Animal)
                .Include(s => s.Photo)
                .FirstOrDefaultAsync(m => m.StoryId == id);

            if (successStory == null)
            {
                return NotFound();
            }

            return View(successStory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var successStory = await _context.SuccessStories.FindAsync(id);
            if (successStory != null)
            {
                _context.SuccessStories.Remove(successStory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}