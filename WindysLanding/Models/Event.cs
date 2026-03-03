using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindysLanding.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Event Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Event Date")]
        public DateTime Date { get; set; }

        [StringLength(500)]
        [Display(Name = "Event URL")]
        public string? EventUrl { get; set; }

        [StringLength(500)]
        [Display(Name = "Event Image")]
        public string? EventImg { get; set; }

        // Navigation Properties
        [Display(Name = "Photos")]
        public virtual ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
    }
}
