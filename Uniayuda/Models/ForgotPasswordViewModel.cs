using Cross;
using System.ComponentModel.DataAnnotations;

namespace Uniayuda.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "The new password is required")]
        [StringLength(100, ErrorMessage = "The password must be contains at least {2} characters", MinimumLength = Constants.passwordMinimumLength)]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "The new password confirmation is required")]
        [Display(Name = "Repeat New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match")]
        public string RepeatNewPassword { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class FillForgotPasswordViewModel
    {
        [Required(ErrorMessage = "The email or username is required")]
        [Display(Name = "Email or username")]
        [StringLength(200, ErrorMessage = "The email or username cannot contain more than {1} characters")]
        public string EmailOrUsername { get; set; }
    }
}