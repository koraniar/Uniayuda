using Entities.Enums;
using System;

namespace Entities.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public double Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public PurchaseStatus State { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
