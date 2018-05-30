using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DatabaseEntities
{
    [Table("History")]
    public class History
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        public History()
        {
            Id = Guid.NewGuid();
        }
    }
}
