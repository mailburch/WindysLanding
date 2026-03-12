using System.ComponentModel.DataAnnotations;

namespace WindysLanding.Models
{
    public class ContactFormViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Subject { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;
    }
}