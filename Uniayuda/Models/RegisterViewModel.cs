using Cross;
using System.ComponentModel.DataAnnotations;

namespace Uniayuda.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The username is required")]
        [Display(Name = "Username")]
        [StringLength(30, ErrorMessage = "The username cannot contain more than {1} characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The email is required")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(200, ErrorMessage = "The email cannot contain more than {1} characters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The password is required")]
        [StringLength(100, ErrorMessage = "The password must be contains at least {2} characters", MinimumLength = Constants.passwordMinimumLength)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "The password confirmation is required")]
        [Display(Name = "Repat Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        public string RepeatPassword { get; set; }
    }
}