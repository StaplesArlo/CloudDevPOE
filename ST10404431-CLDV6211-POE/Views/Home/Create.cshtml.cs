using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST10404431CLDV6211POE.Models;

namespace ST10404431CLDV6211POE.Views.Home
{
    public class CreateModel : PageModel
    {
        private readonly ST10404431CLDV6211POE.Models.EventEaseDBContext _context;

        public CreateModel(ST10404431CLDV6211POE.Models.EventEaseDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Venue Venue { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Venues.Add(Venue);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
