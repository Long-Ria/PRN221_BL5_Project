using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;

namespace OnlineMedicine.Pages.Medicines
{
    public class DetailsModel : PageModel
    {
        private readonly ProjectPRN211_HuongNT7_G6Context _context;

        public DetailsModel(ProjectPRN211_HuongNT7_G6Context context)
        {
            _context = context;
        }
        public Medicine MedicineX { get; set; }
        public List<Medicine> medicines { get; set; }
        public IActionResult OnGet(int? id)
        {
            Medicine m = _context.Medicines.Include(x => x.Country).
                            Include(x => x.Type).Include(x => x.Category).
                            FirstOrDefault(x => x.Id == id);
            List<Medicine> listM = _context.Medicines.Where(x => x.Id != id).OrderBy(x => Guid.NewGuid()).Take(3).ToList();
            MedicineX = m;
            medicines = listM;
            return Page();
        }
    }
}
