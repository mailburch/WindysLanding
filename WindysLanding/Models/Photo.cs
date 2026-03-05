using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindysLanding.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }

        // Foreign Keys 
        [Display(Name = "Animal")]
        public int? AnimalId { get; set; }

        [Display(Name = "Sponsor Company")]
        public int? SponsorCompanyId { get; set; }

        [Display(Name = "Newsletter")]
        public int? NewsletterId { get; set; }

        [Display(Name = "Event")]
        public int? EventId { get; set; }

        [StringLength(500)]
        [Display(Name = "Image URL")]
        public string? ImgUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "Caption")]
        public string? Caption { get; set; }

        // Navigation Properties
        [ForeignKey("AnimalId")]
        public virtual Animal? Animal { get; set; }

        [ForeignKey("SponsorCompanyId")]
        public virtual SponsorCompany? SponsorCompany { get; set; }

        [ForeignKey("NewsletterId")]
        public virtual Newsletter? Newsletter { get; set; }

        [ForeignKey("EventId")]
        public virtual Event? Event { get; set; }
    }
}
