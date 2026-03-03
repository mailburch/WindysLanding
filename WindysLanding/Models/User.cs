using System.ComponentModel.DataAnnotations;

namespace WindysLanding.Models
{
    // User model for Google Authentication
    // Admin is handled separately (not in database)
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Subscribe to Newsletter")]
        public bool NewsletterOptIn { get; set; } = false;
    }
}
