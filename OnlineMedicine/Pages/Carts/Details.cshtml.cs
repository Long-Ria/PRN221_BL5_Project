using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;

namespace OnlineMedicine.Pages.Carts
{
    public class DetailsModel : PageModel
    {
        
        public Account User { get; set; }

        public List<Cart> listM {  get; set; }

        private readonly ProjectPRN211_HuongNT7_G6Context _context = new ProjectPRN211_HuongNT7_G6Context();
        public int GetId()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (userJson != null)
            {
                User = System.Text.Json.JsonSerializer.Deserialize<Account>(userJson);
                return User.Id;
            }
            else
            {
                return -1;
            }
        }
        public IActionResult OnGet()
        {
            int accId = GetId();
            List<Cart> list = _context.Carts.Include(x => x.Account).
                Include(x => x.Medicine).Include(x => x.Medicine.Type).Include(x => x.Medicine.Category).
                Include(x => x.Medicine.Country).
                Where(x => x.AccountId == accId).ToList();
            listM = list;
            return Page();
        }

        public IActionResult OnGetUpdateQuantity(int quantity, int mediId, decimal unitPrice)
        {
            int accId = GetId();

            Cart c = _context.Carts.FirstOrDefault(x => x.AccountId == accId && x.MedicineId == mediId);
            c.Quantity = quantity;
            c.Price = unitPrice * quantity;
            _context.Carts.Update(c);
            _context.SaveChanges();


            return RedirectToPage("/Carts/Details");
        }

        public IActionResult OnGetDeleteOneItemInCart(int id)
        {
            int accId = GetId();
            Cart c = _context.Carts.FirstOrDefault(x => x.AccountId == accId && x.MedicineId == id);
            _context.Carts.Remove(c);
            _context.SaveChanges();

            return RedirectToPage("/Carts/Details");
        }
        public IActionResult OnPostPaypalPayment(string listId = null)
        {
            int accountId = GetId();
            if (string.IsNullOrEmpty(listId))
            {
                return RedirectToPage("/Carts/Details");
            }

            HttpContext.Session.SetString("listId", listId);
            return RedirectToPage("/Carts/CheckOutInfo");
        }
    }
}
