using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindysLanding.Models
{
    public class SuccessStory
    {
        [Key]
        public int StoryId { get; set; }

        [Required]
        [Display(Name = "Animal")]
        public int AnimalId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Story Content")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Date Published")]
        public DateTime DatePublished { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("AnimalId")]
        public virtual Animal? Animal { get; set; }

        [Display(Name = "Photos")]
        public virtual ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
    }
}
