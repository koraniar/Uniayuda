using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum PurchaseStatus
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Created")]
        Created = 1,
        [Display(Name = "Approved")]
        Approved = 2,
        [Display(Name = "Pending")]
        Pending = 3,
        [Display(Name = "Rejected")]
        Rejected = 4
    }
}
