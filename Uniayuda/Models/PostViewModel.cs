using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Uniayuda.Models
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        [Required(ErrorMessage = "The title is required")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        [Display(Name = "Publish as Anonymous")]
        public bool IsAnonymous { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsEdition { get; set; }
        public double AssesmentAverage { get; set; }
        public int UserAssesment { get; set; }
        public List<CommentViewModel> Comments { get; set; }

        public PostViewModel()
        {
            Comments = new List<CommentViewModel>();
        }
    }
}