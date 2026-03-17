using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WindysLanding.Models
{
    // ApplicationUser extends IdentityUser for Google Authentication
    // Includes custom fields for newsletter opt-in
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Full Name")]
        [StringLength(100)]
        public string? FullName { get; set; }

        [Display(Name = "Subscribe to Newsletter")]
        public bool NewsletterOptIn { get; set; } = false;

        // Note: Email, UserName, etc. are inherited from IdentityUser
        // Id is string (not int) when using Identity
    }
}