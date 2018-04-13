using Cross;
using System.ComponentModel.DataAnnotations;

namespace Uniayuda.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "The current password is required")]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
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
    }
}