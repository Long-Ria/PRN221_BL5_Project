using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OnlineMedicine.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string? ReturnUrl { get; set; }
        public void OnGet()
        {
        }
    }
}
