using System.ComponentModel.DataAnnotations;

namespace Uniayuda.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The email is required")]
        [Display(Name = "Email")]
        [StringLength(200, ErrorMessage = "The email cannot contain more than {1} characters")]
        public string UsernameEmail { get; set; }
        [Required(ErrorMessage = "The password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
    }
}