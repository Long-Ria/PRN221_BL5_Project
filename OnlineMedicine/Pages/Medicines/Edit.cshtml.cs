using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OnlineMedicine.Pages.Medicines
{
    public class EditModel : PageModel
    {
        private readonly ProjectPRN211_HuongNT7_G6Context _context;

        public EditModel(ProjectPRN211_HuongNT7_G6Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Medicine Medicine { get; set; } = default!;
        public List<Category> Categories { get; set; }
        public List<Models.Type> Types { get; set; }
        public List<Country> Countries { get; set; }

        public IActionResult OnGet(int mid)
        {

            Medicine m = _context.Medicines.Include(x => x.Category)
                .Include(x => x.Type).Include(x => x.Country)
                .FirstOrDefault(x => x.Id == mid);
            Categories = _context.Categories.Where(x => x.Id != m.CategoryId).ToList();
            Types = _context.Types.Where(x => x.Id != m.TypeId).ToList();
            Countries = _context.Countries.Where(x => x.Id != m.CountryId).ToList();

            Medicine = m;
            return Page();

        }

        public IActionResult OnPost(string name, int category, DateTime expriedDate, string image, string descript, int minAge,
           int typeId, int country, decimal price, int quantity, int mid)
        {


            try
            {

                int start = image.LastIndexOf("<img src=\"");
                int end = image.LastIndexOf("\" alt=\"");
                string imgLink = image.Substring(start + 10, end - (start + 10));
                Medicine m = _context.Medicines.Include(x => x.Category)
                    .Include(x => x.Type).Include(x => x.Country).FirstOrDefault(x => x.Id == mid);
                m.Name = name;
                m.CategoryId = category;
                m.ExpiredDate = expriedDate;
                m.Image = imgLink;
                m.Descript = descript;
                m.MinAge = minAge;
                m.TypeId = typeId;
                m.Price = price;
                m.Quantity = quantity;
                m.CountryId = country;
                if (_context.SaveChanges() >= 0)
                {
                    m = _context.Medicines.Include(x => x.Category)
                   .Include(x => x.Type).Include(x => x.Country).FirstOrDefault(x => x.Id == mid);
                    Categories = _context.Categories.Where(x => x.Id != m.CategoryId).ToList();
                    Types = _context.Types.Where(x => x.Id != m.TypeId).ToList();
                    Countries = _context.Countries.Where(x => x.Id != m.CountryId).ToList();

                    Medicine = m;
                    return Page();
                }



                TempData["Success"] = "Medicine updated successfully!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {

                TempData["Error"] = "Concurrency error occurred while updating the medicine.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                TempData["Error"] = $"Error updating medicine: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }
    }
}
