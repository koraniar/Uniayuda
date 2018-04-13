using System;
using System.Collections.Generic;

namespace Entities.Entities
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual IList<User> Users { get; set; }

        public Country()
        {
            Users = new List<User>();
        }
    }
}
