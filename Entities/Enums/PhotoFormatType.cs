using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum PhotoFormatType
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Image")]
        Image = 1,
        [Display(Name = "Gif")]
        Gif = 2
    }
}
