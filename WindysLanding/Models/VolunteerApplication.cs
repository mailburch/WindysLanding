using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindysLanding.Models
{
    public class VolunteerApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        [Display(Name = "Why do you want to volunteer?")]
        public string? VolunteerDescription { get; set; }

        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Display(Name = "Waiver Signed")]
        public bool WaiverSigned { get; set; } = false;

        [Display(Name = "Waiver Signed Date")]
        public DateTime? WaiverSignedDate { get; set; }
    }
}
