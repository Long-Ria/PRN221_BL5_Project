using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;
using PayPal.Api;

namespace OnlineMedicine.Pages.Carts
{
    public class CheckOutModel : PageModel
    {
        public Account User { get; set; }
        
        public string messageErr { get; set; }

        public Models.Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        private readonly ProjectPRN211_HuongNT7_G6Context _context = new ProjectPRN211_HuongNT7_G6Context();
        public void OnGet()
        {
        }
        public IActionResult CheckOut()
        {

            int accountId = GetId();
            if (HttpContext.Session.GetString("listId") == null)
            {
                return Redirect("/Carts/Details/");
            }
            var listId = HttpContext.Session.GetString("listId");
            string[] s = listId.Split('-');

            List<Cart> carts = _context.Carts.Where(x => x.AccountId == accountId && s.Contains(x.MedicineId.ToString())).ToList();
            Models.Order order = new Models.Order();


            decimal totalMoney = 0;
            foreach (Cart c in carts)
            {
                Medicine product = _context.Medicines.FirstOrDefault(x => x.Id == c.MedicineId);
                if (product.Quantity < c.Quantity)
                {
                    messageErr = "Some medicines are out stock.";
                    return RedirectToPage("/Medicines");
                }
                totalMoney += c.Price;
            }
            order.TotalMoney = totalMoney;
            order.CreatedDate = DateTime.Now;
            order.AccountId = accountId;
            order.CustomerName = HttpContext.Session.GetString("customerName");
            order.Address = HttpContext.Session.GetString("customerAddress");
            order.PhoneNumber = HttpContext.Session.GetString("customerPhoneNumber");
            _context.Orders.Add(order);
            if (_context.SaveChanges() > 0)
            {
                List<OrderDetail> listDetail = new List<OrderDetail>();
                List<Medicine> listProduct = new List<Medicine>();
                int orderId = _context.Orders.OrderBy(order => order.Id).Last().Id;
                foreach (Cart c in carts)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.OrderId = orderId;
                    detail.MedicineId = c.MedicineId;
                    detail.Quantity = c.Quantity;
                    detail.Price = c.Price;
                    listDetail.Add(detail);
                    Medicine product = _context.Medicines.FirstOrDefault(x => x.Id == detail.MedicineId);
                    product.Quantity = product.Quantity - c.Quantity;
                    listProduct.Add(product);
                }
                _context.OrderDetails.AddRange(listDetail);
                _context.SaveChanges();
                _context.Medicines.UpdateRange(listProduct);
                _context.SaveChanges();

                _context.Carts.RemoveRange(carts);
                _context.SaveChanges();


            }
            Order = _context.Orders.Where(x => x.AccountId == order.AccountId).Include(x => x.Account).OrderBy(order => order.Id).Last();
            OrderDetails = _context.OrderDetails.Where(x => x.OrderId == order.Id)
                .Include(x => x.Medicine).ToList();
            HttpContext.Session.Remove("listId");
            HttpContext.Session.Remove("customerName");
            HttpContext.Session.Remove("customerAddress");
            HttpContext.Session.Remove("customerPhoneNumber");
            return RedirectToPage("/Carts/CheckOut");
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



        
        
        public IActionResult OnPostAddToCart(int medicineId, int quantity, decimal unitPrice)
        {
            int accId = GetId();
            Cart c;

            if (_context.Carts.FirstOrDefault(x => x.AccountId == accId && x.MedicineId == medicineId) == null)
            {
                c = new Cart
                {
                    AccountId = accId,
                    Quantity = quantity,
                    MedicineId = medicineId,
                    Price = quantity * unitPrice
                };
                //trong cart chưa có thì add
                _context.Carts.Add(c);
                _context.SaveChanges();
            }
            else
            {
                //có rồi thì update lại thông tin
                c = _context.Carts.FirstOrDefault(x => x.AccountId == accId && x.MedicineId == medicineId);
                c.AccountId = accId;
                c.Quantity = quantity;
                c.MedicineId = medicineId;
                c.Price = quantity * unitPrice;
                _context.Carts.Update(c);
                _context.SaveChanges();
            }

            return Redirect("/Medicines/Details?id=" + medicineId);

        }
    }
}
