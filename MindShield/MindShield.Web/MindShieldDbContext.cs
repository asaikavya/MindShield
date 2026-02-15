using Microsoft.EntityFrameworkCore;
using MindShield.Core;

namespace MindShield.Web
{
    public class MindShieldDbContext : DbContext
    {
        public MindShieldDbContext(DbContextOptions<MindShieldDbContext> options)
            : base(options)
        {
        }

        public DbSet<RealityProfile> RealityProfiles { get; set; }

        // --- THE FIX IS HERE ---
        // We added "internal" to match the base class signature
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data (Optional - keeps your test user ready)
            modelBuilder.Entity<RealityProfile>().HasData(
            new RealityProfile
            {
                Id = 1,
                UserId = "user_01",
                FullName = "Alex Rivera",
                CurrentJobTitle = "Senior Project Manager",
                Employer = "TechFlow Solutions",
                HomeLocation = "San Jose, CA",
                IsGuardianActive = true,
                // FIX: Use a hardcoded date so it doesn't change every second
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
        }
    }
}