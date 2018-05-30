using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace Entities.DatabaseEntities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? BornDate { get; set; }
        public DateTime? LastEmailResended { get; set; }
        public DateTime? LastTimePasswordRestored { get; set; }
        public virtual List<Assessment> GivenAssessments { get; set; }
        public virtual List<Comment> GivenComments { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<History> History { get; set; }

        public User() {
            Id = Guid.NewGuid().ToString();
            GivenAssessments = new List<Assessment>();
            GivenComments = new List<Comment>();
            Posts = new List<Post>();
            History = new List<History>();
        }
    }
}
