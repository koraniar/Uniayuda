using System;
using System.ComponentModel.DataAnnotations;

namespace Uniayuda.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Born date")]
        public DateTime BornDate { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public bool IsFromDashboard { get; set; }
    }
}