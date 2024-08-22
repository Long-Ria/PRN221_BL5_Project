using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMedicine.Models;
using PayPal.Api;
using System.Xml.Linq;

namespace OnlineMedicine.Pages.Carts
{
    public class CheckOutInfoModel : PageModel
    {
        public Account User { get; set; }
        public List<Cart> Carts { get; set; }
        private readonly ProjectPRN211_HuongNT7_G6Context _context = new ProjectPRN211_HuongNT7_G6Context();


        private PayPal.Api.Payment payment;


        public IActionResult OnGet()
        {
            
                int accountId = GetId();
                Models.Account acc = _context.Accounts.FirstOrDefault(x => x.Id == accountId);
                User = acc;

                if (HttpContext.Session.GetString("listId") == null)
                {
                    return Redirect("/Carts/Details");
                }
                var listId = HttpContext.Session.GetString("listId");
                string[] s = listId.Split('-');

                List<Cart> carts = _context.Carts.Where(x => x.AccountId == accountId && s.Contains(x.MedicineId.ToString()))
                    .Include(x => x.Medicine)
                    .Include(x => x.Medicine.Type)
                    .Include(x => x.Medicine.Category)
                    .Include(x => x.Medicine.Country).ToList();
                Carts = carts;

                return Page();
            
            
        }
        public ActionResult OnGetPaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Query["PayerID"].ToString();
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}/Carts/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    HttpContext.Session.SetString(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Query["guid"].ToString();
                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString(guid) as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("FailureView");
            }
            //on successful payment, show success page to user.  
            return RedirectToPage("/Carts/CheckOut");
        }
        public ActionResult OnPostPaymentWithPaypal(string Cancel = null, string name = null, string address = null, string phoneNumber = null)
        {
            if (name == null || address == null || phoneNumber == null)
            {
                return Page();
            }
                HttpContext.Session.SetString("customerName", name);
                HttpContext.Session.SetString("customerAddress", address);
                HttpContext.Session.SetString("customerPhoneNumber", phoneNumber);
            
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Query["PayerID"].ToString();
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}/Carts/CheckOutInfo?handler=PaymentWithPayPal&";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    HttpContext.Session.SetString(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Query["guid"].ToString();
                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString(guid) as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("FailureView");
            }
            //on successful payment, show success page to user.  
            return RedirectToPage("/Carts/CheckOut");
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {

            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            List<Cart> carts = new List<Cart>();
            if (HttpContext.Session.GetString("listId") != null)
            {
                var listId = HttpContext.Session.GetString("listId");
                int accountId = GetId();
                string[] s = listId.Split('-');

                carts = _context.Carts.Where(x => x.AccountId == accountId && s.Contains(x.MedicineId.ToString()))
                    .Include(x => x.Medicine).ToList();
            }
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            //Adding Item Details like name, currency, price etc
            decimal totalMoney = 0;
            foreach (Cart c in carts)
            {
                totalMoney += c.Price;
                itemList.items.Add(new Item()
                {
                    name = c.Medicine.Name,
                    currency = "USD",
                    price = decimal.Round(c.Medicine.Price, 0).ToString(),
                    quantity = c.Quantity.ToString(),
                    sku = "sku"
                });
            }
            totalMoney = decimal.Round(totalMoney, 0);
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = totalMoney.ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = totalMoney.ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
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
