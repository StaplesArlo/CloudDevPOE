#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10404431CLDV6211POE.Models;

namespace ST10404431CLDV6211POE.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventEaseDBContext _context;

        public EventsController(EventEaseDBContext context)
        {
            _context = context;
        }

        //-------------------- GET: Events (with Pagination & Search) --------------------//
        public async Task<IActionResult> EventsIndex(string searchTerm, int page = 1, int pageSize = 10)
        {
            var query = _context.Events.Include(e => e.Venue).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.EventName.Contains(searchTerm) || e.Description.Contains(searchTerm));
            }

            var events = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["SearchTerm"] = searchTerm;
            return View(events);
        }

        //-------------------- GET: Events/Details --------------------//
        public async Task<IActionResult> EventDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (@event == null) return NotFound();
            return View(@event);
        }

        //-------------------- GET: Create New Event --------------------//
        public IActionResult NewEvent()
        {
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName"); 
            return View();
        }

        //-------------------- POST: Create New Event --------------------//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewEvent([Bind("VenueID, EventName, EventDate, Description")] Event @event)
        {
            // Validate duplicate event
            if (_context.Events.Any(e => e.EventName == @event.EventName && e.EventDate == @event.EventDate))
            {
                ModelState.AddModelError("EventName", "An event with this name and date already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EventsIndex));
            }

            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", @event.VenueID);
            return View(@event);
        }

        //-------------------- GET: Update Event --------------------//
        public async Task<IActionResult> UpdateEvent(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", @event.VenueID);
            return View(@event);
        }

        //-------------------- POST: Update Event --------------------//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEvent(string id, [Bind("EventID,VenueID,EventName,EventDate,Description")] Event @event)
        {
            if (id != @event.EventID)
            {
                Console.WriteLine($"Update failed: Mismatched Event ID ({id})");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(EventsIndex));
            }

            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", @event.VenueID);
            return View(@event);
        }

        //-------------------- GET: Delete Event --------------------//
        public async Task<IActionResult> DeleteEvent(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var @event = await _context.Events
               .Include(e => e.Venue)
               .FirstOrDefaultAsync(m => m.EventID == id);

            if (@event == null) return NotFound();
            return View(@event);
        }

        //-------------------- POST: Delete Event --------------------//
        [HttpPost, ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(EventsIndex));
        }

        //-------------------- Check if Event Exists --------------------//
        private bool EventExists(string id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }
    }
}