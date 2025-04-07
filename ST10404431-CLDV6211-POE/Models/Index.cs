using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ST10404431CLDV6211POE.Models
{
    public class IndexModel : PageModel
    {
        private readonly EventEaseDBContext _context;

        public IndexModel(EventEaseDBContext context)
        {
            _context = context;
        }

        public IList<Venue> Venue { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Venue = await _context.Venues.ToListAsync();
        }
    }
}
