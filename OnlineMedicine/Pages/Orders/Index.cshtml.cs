using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnlineMedicine.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly ProjectPRN211_HuongNT7_G6Context _context = new ProjectPRN211_HuongNT7_G6Context();

        public Account User { get; set; }

        public List<Order> listO {  get; set; }

        public DateTime FromDate {  get; set; }
        public DateTime ToDate {  get; set; }
        public int Sort {  get; set; }

        public IActionResult OnGet(DateTime? fromDate, DateTime? toDate, string? sort)
        {

            if (fromDate != null)
            {
                FromDate = fromDate.Value;
            }
            if (toDate != null)
            {
                ToDate = toDate.Value;
            }
            if (sort != null)
            {
                Sort = int.Parse(sort);
            }

            int n = GetId();
            Account account = _context.Accounts.Include(x => x.Role).FirstOrDefault(u => u.Id == n);
            if (account == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Order> orders = new List<Order>();
            orders = _context.Orders.Where(x => x.AccountId == account.Id)
            .Include(x => x.Account).ToList();


            if (FromDate.Year >= 2000)
            {
                orders = orders.Where(x => x.AccountId == account.Id && FromDate.CompareTo(x.CreatedDate) <= 0).ToList();
            }
            if (ToDate.Year >= 2000)
            {
                orders = orders.Where(x => x.AccountId == account.Id && ToDate.CompareTo(x.CreatedDate) >= 0).ToList();
            }
            if (Sort == 1)
            {
                orders = orders.OrderByDescending(x => x.CreatedDate).ToList();
            }
            if (Sort == 2)
            {
                orders = orders.OrderBy(x => x.CreatedDate).ToList();
            }

            listO = orders;
            return Page();
        }

        

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
    }
}
