using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using OnlineMedicine.Hubs;
using OnlineMedicine.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMedicine.Pages.Medicines
{

    public class CreateModel : PageModel
    {
        private readonly ProjectPRN211_HuongNT7_G6Context _context;
        private readonly IHubContext<SignalrServer> _hubContext;

        public CreateModel(ProjectPRN211_HuongNT7_G6Context context, IHubContext<SignalrServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Models.Medicine medicines { get; set; }
        public List<Category> Categories { get; set; }
        public List<Models.Type> Types { get; set; }
        public List<Country> Countries { get; set; }

        public void OnGet()
        {
            Categories = _context.Categories.ToList();
            Types = _context.Types.ToList();
            Countries = _context.Countries.ToList();
        }

        public IActionResult OnPost(string name, int category, DateTime expriedDate, string image, string descript, int minAge,
            int typeId, int country, int price, int quantity)
        {
            

            try
            {
                // Process the image string as in the original controller
                if (string.IsNullOrEmpty(medicines.Image)) throw new Exception();

                int start = medicines.Image.LastIndexOf("<img src=\"");
                int end = medicines.Image.LastIndexOf("\" alt=\"");
                string imgLink = medicines.Image.Substring(start + 10, end - (start + 10));
                Medicine m = new Medicine();
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
                _context.Medicines.Add(m);
                _context.SaveChanges();

                 

                return RedirectToPage("List");
            }
            catch
            {
                Categories = _context.Categories.ToList();
                Types = _context.Types.ToList();
                Countries = _context.Countries.ToList();
                ModelState.AddModelError(string.Empty, "Create new failed");
                return Page();
            }
        }
    }
}
