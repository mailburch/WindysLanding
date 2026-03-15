using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WindysLanding.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events
                .Include(e => e.Photos)
                .ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Photos)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Name,Description,Date,EventUrl")] Event @event, IFormFile? ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (JPG, PNG, GIF, WEBP) are allowed");
                }
            }

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "events");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    @event.EventImg = "/images/events/" + fileName;
                }

                _context.Add(@event);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(@event.EventImg))
                {
                    var photo = new Photo
                    {
                        EventId = @event.EventId,
                        ImgUrl = @event.EventImg,
                        Caption = @event.Name
                    };

                    _context.Photos.Add(photo);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Photos)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("EventId,Name,Description,Date,EventUrl,EventImg")] Event @event,
            IFormFile? ImageFile,
            bool RemovePhoto)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (JPG, PNG, GIF, WEBP) are allowed");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEvent = await _context.Events
                        .Include(e => e.Photos)
                        .FirstOrDefaultAsync(e => e.EventId == id);

                    if (existingEvent == null)
                    {
                        return NotFound();
                    }

                    existingEvent.Name = @event.Name;
                    existingEvent.Description = @event.Description;
                    existingEvent.Date = @event.Date;
                    existingEvent.EventUrl = @event.EventUrl;

                    if (RemovePhoto)
                    {
                        existingEvent.EventImg = null;

                        var existingPhotoRecords = existingEvent.Photos.ToList();
                        foreach (var photo in existingPhotoRecords)
                        {
                            _context.Photos.Remove(photo);
                        }
                    }

                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                        var fileName = Guid.NewGuid().ToString() + extension;
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "events");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        var imagePath = "/images/events/" + fileName;
                        existingEvent.EventImg = imagePath;

                        var oldPhotoRecords = existingEvent.Photos.ToList();
                        foreach (var photo in oldPhotoRecords)
                        {
                            _context.Photos.Remove(photo);
                        }

                        var newPhoto = new Photo
                        {
                            EventId = existingEvent.EventId,
                            ImgUrl = imagePath,
                            Caption = existingEvent.Name
                        };

                        _context.Photos.Add(newPhoto);
                    }
                    else
                    {
                        foreach (var photo in existingEvent.Photos)
                        {
                            photo.Caption = existingEvent.Name;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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

            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Photos)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Photos)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (@event != null)
            {
                var relatedPhotos = @event.Photos.ToList();
                foreach (var photo in relatedPhotos)
                {
                    _context.Photos.Remove(photo);
                }

                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}