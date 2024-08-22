using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;

namespace OnlineMedicine.Pages.Orders
{
    public class OrderListModel : PageModel
    {
        private readonly ProjectPRN211_HuongNT7_G6Context _context;

        public OrderListModel(ProjectPRN211_HuongNT7_G6Context context)
        {
            _context = context;
        }

        public IList<Order> Orders { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Sort { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Order> ordersQuery = _context.Orders.Include(o => o.OrderDetails);

            if (FromDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreatedDate >= FromDate.Value);
            }

            if (ToDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreatedDate <= ToDate.Value);
            }

            if (Sort.HasValue)
            {
                ordersQuery = Sort == 1
                    ? ordersQuery.OrderBy(o => o.CreatedDate)
                    : ordersQuery.OrderByDescending(o => o.CreatedDate);
            }

            Orders = await ordersQuery.ToListAsync();
        }

        public async Task<IActionResult> OnPostFilterAsync(DateTime? fromDate, DateTime? toDate, int? sort)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Sort = sort;


            return RedirectToPage();
        }
    }
}
