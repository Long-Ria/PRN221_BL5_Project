using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineMedicine.Pages.Carts
{
    public class DetailsModel : PageModel
    {
        [BindProperty]
        public double totalOrder {  get; set; }
        public void OnGet()
        {

        }
    }
}
