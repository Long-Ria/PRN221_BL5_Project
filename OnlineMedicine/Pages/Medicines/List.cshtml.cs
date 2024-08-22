using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMedicine.Pages.Medicines
{
    public class ListModel : PageModel
    {
        private readonly ProjectPRN211_HuongNT7_G6Context _context;

        public ListModel(ProjectPRN211_HuongNT7_G6Context context)
        {
            _context = context;
        }

        public IList<Medicine> Medicines { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }

        public IActionResult OnGet(string sortField = "Name", string sortOrder = "asc")
        {
            SortField = sortField;
            SortOrder = sortOrder;

            IQueryable<Medicine> medicinesQuery = _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Country)
                .Include(m => m.Type);

            switch (SortField)
            {
                case "Name":
                    medicinesQuery = SortOrder == "asc" ? medicinesQuery.OrderBy(m => m.Name) : medicinesQuery.OrderByDescending(m => m.Name);
                    break;
                case "ExpiredDate":
                    medicinesQuery = SortOrder == "asc" ? medicinesQuery.OrderBy(m => m.ExpiredDate) : medicinesQuery.OrderByDescending(m => m.ExpiredDate);
                    break;
            }

            Medicines = medicinesQuery.ToList();
            return Page();

        }
        public IActionResult OnGetDelete(int mid)
        {

            Medicine m = _context.Medicines.FirstOrDefault(x => x.Id == mid);
            m.Quantity = 0;
            if (_context.SaveChanges() >= 0)
            {
                Medicines = _context.Medicines.Include(x => x.Category).Include(x => x.Type).Include(x => x.Country).ToList();
                return RedirectToPage("/Medicines/List");
            }
            Medicines = _context.Medicines.Include(x => x.Category).Include(x => x.Type).Include(x => x.Country).ToList();


            return RedirectToPage("/Medicines/List");
        }
    }
}
