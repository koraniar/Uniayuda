using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum AssessmentLevel
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "One")]
        One = 1,
        [Display(Name = "Two")]
        Two = 2,
        [Display(Name = "Three")]
        Three = 3,
        [Display(Name = "Four")]
        Four = 4,
        [Display(Name = "Five")]
        Five = 5
    }
}
