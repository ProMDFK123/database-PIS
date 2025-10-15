using bolsafeucn_back.src.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<GeneralUser, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Image> Images { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }

        //public DbSet<Offer> Ofertas { get; set; }
        //public DbSet<JobApplication> Postulaciones { get; set; }
        //public DbSet<Review> Evaluaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder
                .Entity<Student>()
                .HasOne(s => s.GeneralUser)
                .WithOne()
                .HasForeignKey<Student>(s => s.GeneralUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Company>()
                .HasOne(c => c.GeneralUser)
                .WithOne()
                .HasForeignKey<Company>(c => c.GeneralUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Admin>()
                .HasOne(a => a.GeneralUser)
                .WithOne()
                .HasForeignKey<Admin>(a => a.GeneralUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Individual>()
                .HasOne(i => i.GeneralUser)
                .WithOne()
                .HasForeignKey<Individual>(i => i.GeneralUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
