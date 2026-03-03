using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindysLanding.Models
{
    public class ContactInfo
    {
        [Key]
        public int ContactInfoId { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Physical Address")]
        public string? Address { get; set; }
    }
}
