#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10404431CLDV6211POE.Models;

namespace ST10404431CLDV6211POE.Controllers
{
    public class VenuesController : Controller
    {
        private readonly EventEaseDBContext _context;

        public VenuesController(EventEaseDBContext context)
        {
            _context = context;
        }

        //-------------------- VenuesIndex with Search --------------------//
        public async Task<IActionResult> VenuesIndex(string searchTerm)
        {
            var venues = _context.Venues.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                venues = venues.Where(v => v.VenueName.Contains(searchTerm) || v.Location.Contains(searchTerm));
            }

            return View(await venues.ToListAsync());
        }

        //-------------------- VenueDetails --------------------//
        public async Task<IActionResult> VenueDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        //-------------------- AddVenue --------------------//
        public IActionResult AddVenue()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVenue([Bind("VenueID,VenueName,Location,Capacity,ImageURL,Category")] Venue venue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(VenuesIndex));
            }
            return View(venue);
        }

        //-------------------- UpdateVenue --------------------//
        public async Task<IActionResult> UpdateVenue(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVenue(string id, [Bind("VenueID,VenueName,Location,Capacity,ImageURL,Category")] Venue venue)
        {
            if (id != venue.VenueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(VenuesIndex));
            }

            return View(venue);
        }
        //-------------------- DeleteVenue --------------------//
        public async Task<IActionResult> DeleteVenue(string id)
        {
            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        [HttpPost, ActionName("DeleteVenue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(VenuesIndex));
        }

        private bool VenueExists(string id)
        {
            return _context.Venues.Any(e => e.VenueID == id);
        }
    }
}