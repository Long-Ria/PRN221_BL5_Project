
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using OnlineMedicine.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace OnlineMedicine.Pages.Accounts
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [Required]
        [BindProperty]
        public string Username { get; set; }
        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string? ReturnUrl { get; set; }

        private readonly ProjectPRN211_HuongNT7_G6Context _context;

        public LoginModel(ProjectPRN211_HuongNT7_G6Context context)
        {
            _context = context;
        }

        public void OnGet(string returnUrl = "")
        {
            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var acc = _context.Accounts
                    .Where(a => a.Username == Username
                                && a.Password == Password)
                    .FirstOrDefault();
                if (acc != null)
                {
                    

                    var userJson = JsonSerializer.Serialize(acc);
                    HttpContext.Session.SetString("User", userJson);
                    //Redirect to last url before login
                    if (ReturnUrl != null && ReturnUrl.Length > 0)
                    {
                        return LocalRedirect(ReturnUrl);
                    }
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Accounts or passwords incorrectly");
                    return Page();
                }
            }
            return Page();

        }
        public  IActionResult OnGetLogout()
        {
            // Sign out the user
            HttpContext.Session.Remove("User");

            // Redirect to the home page
            return RedirectToPage("/Index");
        }
    }
}
