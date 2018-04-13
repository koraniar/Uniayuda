using Entities.Enums;
using System;

namespace Entities.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }
        public PhotoFormatType Format { get; set; }
        public string Path { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
