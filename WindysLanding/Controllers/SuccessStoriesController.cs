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
    public class SuccessStoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuccessStoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SuccessStories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuccessStories.Include(s => s.Animal);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SuccessStories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var successStory = await _context.SuccessStories
                .Include(s => s.Animal)
                .FirstOrDefaultAsync(m => m.StoryId == id);
            if (successStory == null)
            {
                return NotFound();
            }

            return View(successStory);
        }

        // GET: SuccessStories/Create
        public IActionResult Create()
        {
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalType");
            return View();
        }

        // POST: SuccessStories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            return View(successStory);
        }

        // GET: SuccessStories/Edit/5
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
            return View(successStory);
        }

        // POST: SuccessStories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    if (!SuccessStoryExists(successStory.StoryId))
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
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalType", successStory.AnimalId);
            return View(successStory);
        }

        // GET: SuccessStories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var successStory = await _context.SuccessStories
                .Include(s => s.Animal)
                .FirstOrDefaultAsync(m => m.StoryId == id);
            if (successStory == null)
            {
                return NotFound();
            }

            return View(successStory);
        }

        // POST: SuccessStories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var successStory = await _context.SuccessStories.FindAsync(id);
            if (successStory != null)
            {
                _context.SuccessStories.Remove(successStory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuccessStoryExists(int id)
        {
            return _context.SuccessStories.Any(e => e.StoryId == id);
        }
    }
}
