using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Pleasures")]
        public string Pleasures { get; set; }
        [Display(Name = "Sentence")]
        public string Sentence { get; set; }
        [Display(Name = "Personal link")]
        public string PersonalLink { get; set; }
        [Display(Name = "Select profession")]
        public List<SelectListItem> Professions { get; set; }
        [Display(Name = "Profession")]
        public string ProfessionId { get; set; }
        public string ProfessionName { get; set; }
        [Display(Name = "Select country")]
        public List<SelectListItem> Countries { get; set; }
        [Display(Name = "Country")]
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string URLPhoto { get; set; }
        [Display(Name = "Current investment")]
        public double Investment { get; set; }

        public ProfileViewModel()
        {
            Professions = new List<SelectListItem>();
            Countries = new List<SelectListItem>();
        }
    }
}