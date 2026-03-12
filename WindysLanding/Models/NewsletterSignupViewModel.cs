using System.ComponentModel.DataAnnotations;

namespace WindysLanding.Models
{
    public class NewsletterSignupViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}