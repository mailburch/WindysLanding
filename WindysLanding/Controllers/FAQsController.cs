using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Controllers
{
    public class FAQsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FAQsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FAQs (public-facing page with search and category filter)
        public async Task<IActionResult> Index(string? search, string? category)
        {
            var query = _context.FAQs.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(f =>
                    f.Question.Contains(search) ||
                    f.Answer.Contains(search));
            }

            // Apply category filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(f => f.Category == category);
            }

            // Order by DisplayOrder
            query = query.OrderBy(f => f.DisplayOrder);

            // Get distinct categories for the filter tabs
            var categories = await _context.FAQs
                .Where(f => f.Category != null)
                .Select(f => f.Category!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewData["CurrentSearch"] = search;
            ViewData["CurrentCategory"] = category;
            ViewData["Categories"] = categories;

            return View(await query.ToListAsync());
        }

        // GET: FAQs/Manage (ADMIN ONLY - management view)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage()
        {
            var faqs = await _context.FAQs
                .OrderBy(f => f.Category)
                .ThenBy(f => f.DisplayOrder)
                .ToListAsync();

            return View(faqs);
        }

        // GET: FAQs/Details/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQs
                .FirstOrDefaultAsync(m => m.FaqId == id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // GET: FAQs/Create (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: FAQs/Create (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("FaqId,Question,Answer,Category,DisplayOrder")] FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fAQ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(fAQ);
        }

        // GET: FAQs/Edit/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQs.FindAsync(id);
            if (fAQ == null)
            {
                return NotFound();
            }
            return View(fAQ);
        }

        // POST: FAQs/Edit/5 (ADMIN ONLY)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("FaqId,Question,Answer,Category,DisplayOrder")] FAQ fAQ)
        {
            if (id != fAQ.FaqId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fAQ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(fAQ.FaqId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(fAQ);
        }

        // GET: FAQs/Delete/5 (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQs
                .FirstOrDefaultAsync(m => m.FaqId == id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // POST: FAQs/Delete/5 (ADMIN ONLY)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fAQ = await _context.FAQs.FindAsync(id);
            if (fAQ != null)
            {
                _context.FAQs.Remove(fAQ);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        private bool FAQExists(int id)
        {
            return _context.FAQs.Any(e => e.FaqId == id);
        }
    }
}