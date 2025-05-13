using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10404431CLDV6211POE.Models;

namespace ST10404431CLDV6211POE.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEaseDBContext _context;

        public BookingsController(EventEaseDBContext context)
        {
            _context = context;
        }

        //-------------------- Check Availability --------------------//

        // API to check if a venue is available on the selected date
        public JsonResult CheckVenueAvailability(string venueID, DateTime bookingDate)
        {
            if (string.IsNullOrEmpty(venueID))
            {
                return Json(new { success = false, message = "Invalid venue ID." });
            }

            bool isBooked = _context.Bookings.Any(b => b.VenueID == venueID && b.BookingDate == bookingDate);
            return Json(new { success = !isBooked, message = isBooked ? "Venue is already booked." : "Venue available!" });
        }

        // API to get all booked dates for a specific venue
        public JsonResult GetBookedDates(string venueID)
        {
            var bookedDates = _context.Bookings
                .Where(b => b.VenueID == venueID)
                .Select(b => b.BookingDate.ToString("yyyy-MM-dd")) // Format for JavaScript date input
                .ToList();

            return Json(bookedDates);
        }

        //-------------------- Booking Management --------------------//

        public async Task<IActionResult> BookingIndex()
        {
            var eventEaseDBContext = _context.Bookings.Include(b => b.Event).Include(b => b.Venue);
            return View(await eventEaseDBContext.ToListAsync());
        }

        public async Task<IActionResult> BookingDetails(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();
            return View(booking);
        }

        //-------------------- Create Booking --------------------//

        public IActionResult NewBooking()
        {
            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName"); // Show event names
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName"); // Show venue names
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewBooking([Bind("VenueID, EventID, BookingDate")] Booking booking)
        {
            // Validate if the venue is already booked for the selected date
            if (_context.Bookings.Any(b => b.VenueID == booking.VenueID && b.BookingDate == booking.BookingDate))
            {
                ModelState.AddModelError("BookingDate", "This venue is already booked for the selected date.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BookingIndex));
            }

            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        //-------------------- Edit Booking --------------------//

        public async Task<IActionResult> EditBooking(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBooking(int id, [Bind("BookingID,EventID,VenueID,BookingDate")] Booking booking)
        {
            if (id != booking.BookingID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BookingIndex));
            }

            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        //-------------------- Delete Booking --------------------//

        public async Task<IActionResult> DeleteBooking(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost, ActionName("DeleteBooking")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(BookingIndex));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }
    }
}