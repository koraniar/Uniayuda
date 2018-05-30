using Entities.Enums;
using System;

namespace Entities.DatabaseEntities
{
    public class Assessment
    {
        public Guid Id { get; set; }
        public AssessmentLevel Level { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        public Assessment()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
