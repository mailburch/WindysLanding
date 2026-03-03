using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindysLanding.Models
{
    public class FAQ
    {
        [Key]
        public int FaqId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Question")]
        public string Question { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Answer")]
        public string Answer { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;
    }
}
