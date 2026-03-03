using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindysLanding.Models
{
    public class SponsorCompany
    {
        [Key]
        public int SponsorCompanyId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Company Name")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Navigation Properties
        [Display(Name = "Photos")]
        public virtual ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
    }
}
