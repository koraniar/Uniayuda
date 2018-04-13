using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace Entities.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Pleasures { get; set; }
        public string Sentence { get; set; }
        public string PersonalLink { get; set; }
        public DateTime? BornDate { get; set; }
        public DateTime? LastEmailResended { get; set; }
        public DateTime? LastTimePasswordRestored { get; set; }
        public Guid ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }
        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual IList<Purchase> Purchases { get; set; }
        public virtual IList<Photo> Photos { get; set; }

        public User() {
            Id = Guid.NewGuid().ToString();
            Purchases = new List<Purchase>();
            Photos = new List<Photo>();
        }
    }
}
