using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum ResponseStatus
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Success")]
        Success = 1,
        [Display(Name = "Accepted")]
        Accepted = 2,
        [Display(Name = "Warning")]
        Warning = 3,
        [Display(Name = "Error")]
        Error = 4
    }
}
