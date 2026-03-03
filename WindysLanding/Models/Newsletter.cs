using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WindysLanding.Models
{
    public class Newsletter
    {
        [Key]
        public int NewsletterId { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Newsletter URL")]
        public string NewsletterUrl { get; set; } = string.Empty;

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        // Navigation Properties
        [Display(Name = "Photos")]
        public virtual ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
    }
}
