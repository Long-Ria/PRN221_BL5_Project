using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Hubs;
using OnlineMedicine.Models;

namespace OnlineMedicine.Pages.Medicines
{
    public class IndexModel : PageModel
    {
        public string? SearchKeyword { get; set; }
        public List<int>? CategoryIds { get; set; }

        public int SortType { get; set; }

        public int? CountryId { get; set; }
        public List<Medicine> Medicines { get; set; }
        public List<Category> Categories { get; set; }

        public List<Country> Countries { get; set; }

        private readonly ProjectPRN211_HuongNT7_G6Context _context;
        private readonly IHubContext<SignalrServer> _hubContext;

        public IndexModel(ProjectPRN211_HuongNT7_G6Context context, IHubContext<SignalrServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> OnGet()
        {
            List<Medicine> medicines = _context.Medicines.Where(x => x.Quantity > 0)
                                    .Include(m => m.Category)
                                    .Include(m => m.Country)
            .ToList();

            List<Category> categories = _context.Categories
                                        .ToList();

            List<Country> countries = _context.Countries.ToList();

            Medicines = medicines;
            Categories = categories;
            Countries = countries;
            

            return Page();
        }
    }
}
