using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;

namespace OnlineMedicine.Pages.OrderDetails
{
    public class DetailsModel : PageModel


    {
        private readonly ProjectPRN211_HuongNT7_G6Context _context;

        public DetailsModel(ProjectPRN211_HuongNT7_G6Context context)
        {
            _context = context;
        }
        public OrderDetail orderDetails { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
        public IActionResult OnGet(int id)
        {
            OrderDetails = _context.OrderDetails.Where(x => x.OrderId == id).Include(x => x.Medicine)
                .Include(x => x.Order).ThenInclude(x => x.Account)
                .Include(x => x.Medicine.Type).Include(x => x.Medicine.Category).ToList();
            return RedirectToPage("/OrderDetails/Index");
        }
    }
}
