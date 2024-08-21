using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OnlineMedicine.Pages.Accounts
{
    public class RegisterModel : PageModel
    {
        [Required, MinLength(5), MaxLength(50)]
        public string Username { get; set; }
        [Required, MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MinLength(6)]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string RepeatPassword { get; set; }
        [Required]
        public string Address { get; set; }
        [Required(ErrorMessage = "Phone number is required."), MinLength(8), MaxLength(10)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only digits.")]
        [StringLength(10, ErrorMessage = "Phone number cannot be longer than 10 digits.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public void OnGet()
        {
        }
    }
}
