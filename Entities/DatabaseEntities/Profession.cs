using System;
using System.Collections.Generic;

namespace Entities.Entities
{
    public class Profession
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual IList<User> Users { get; set; }

        public Profession()
        {
            Users = new List<User>();
        }
    }
}
