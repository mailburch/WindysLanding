using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WindysLanding.Models
{
    // Changed from DbContext to IdentityDbContext<ApplicationUser>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets - Each one becomes a table in your database
        public DbSet<Animal> Animals { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
        public DbSet<SuccessStory> SuccessStories { get; set; } = null!;
        public DbSet<SponsorCompany> SponsorCompanies { get; set; } = null!;
        public DbSet<VolunteerApplication> VolunteerApplications { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<FAQ> FAQs { get; set; } = null!;
        public DbSet<Newsletter> Newsletters { get; set; } = null!;
        public DbSet<ContactInfo> ContactInfos { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // IMPORTANT: Must call base for Identity tables

            // Configure any specific relationships or constraints here if needed
            // For example, you could configure cascade delete behavior, indexes, etc.
        }
    }
}