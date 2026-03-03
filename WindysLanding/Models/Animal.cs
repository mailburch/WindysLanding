using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindysLanding.Models
{
    public class Animal
    {
        [Key]
        public int AnimalId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Animal Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Animal Type")]
        public string AnimalType { get; set; } = string.Empty; // e.g., "Horse", "Donkey", "Goat", etc.

        [Display(Name = "Age")]
        public int? Age { get; set; }

        [StringLength(50)]
        [Display(Name = "Size")]
        public string? Size { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [StringLength(50)]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        [StringLength(50)]
        [Display(Name = "Breed")]
        public string? Breed { get; set; }

        [StringLength(50)]
        [Display(Name = "Color")]
        public string? Color { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Experience Level Required")]
        public string? Experience { get; set; }

        [Display(Name = "Adoption Status")]
        public bool AdoptionStatus { get; set; } = false; // false = available, true = adopted

        [Display(Name = "Available for Sponsorship")]
        public bool SponsorshipAvailable { get; set; } = true; // true = available for sponsorship, false = currently sponsored

        [StringLength(500)]
        [Display(Name = "Sponsorship URL")]
        public string? SponsorshipUrl { get; set; }

        // Navigation Properties
        [Display(Name = "Photos")]
        public virtual ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();

        [Display(Name = "Success Stories")]
        public virtual ICollection<SuccessStory> SuccessStories { get; set; } = new HashSet<SuccessStory>();
    }
}
