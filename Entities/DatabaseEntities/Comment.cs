using System;

namespace Entities.DatabaseEntities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        public Comment()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
