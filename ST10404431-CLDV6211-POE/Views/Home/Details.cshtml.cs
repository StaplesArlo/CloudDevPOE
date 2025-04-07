using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ST10404431CLDV6211POE.Models;

namespace ST10404431CLDV6211POE.Views.Home
{
    public class DetailsModel : PageModel
    {
        private readonly ST10404431CLDV6211POE.Models.EventEaseDBContext _context;

        public DetailsModel(ST10404431CLDV6211POE.Models.EventEaseDBContext context)
        {
            _context = context;
        }

        public Venue Venue { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueID == id);

            if (venue is not null)
            {
                Venue = venue;

                return Page();
            }

            return NotFound();
        }
    }
}
