using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uniayuda.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string UserName { get; set; }
    }
}