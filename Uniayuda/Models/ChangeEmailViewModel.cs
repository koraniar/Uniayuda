using System.ComponentModel.DataAnnotations;

namespace Uniayuda.Models
{
    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "The new email is required")]
        [Display(Name = "New Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(200, ErrorMessage = "The email cannot contain more than {1} characters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The new email confirmation is required")]
        [Display(Name = "Repeat New Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(200, ErrorMessage = "The email cannot contain more than {1} characters")]
        [Compare("Email", ErrorMessage = "The emails do not match")]
        public string RepeatEmail { get; set; }
        [Required(ErrorMessage = "The password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool hideSubmitButton { get; set; }
    }
}