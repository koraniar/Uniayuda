using System;
using System.Collections.Generic;

namespace Entities.DatabaseEntities
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<Assessment> Assessments { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<History> History { get; set; }

        public Post()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            Assessments = new List<Assessment>();
            Comments = new List<Comment>();
            History = new List<History>();
        }
    }
}
