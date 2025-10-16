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

        public DbSet<Offer> Offers { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        // public DbSet<Review> Reviews { get; set; } // Desactivado temporalmente

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relaciones uno a uno (GeneralUser -> TipoDeUsuario)
            builder
                .Entity<Student>()
                .HasOne(s => s.GeneralUser)
                .WithOne(gu => gu.Student)
                .HasForeignKey<Student>(s => s.GeneralUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Company>()
                .HasOne(c => c.GeneralUser)
                .WithOne(gu => gu.Company)
                .HasForeignKey<Company>(c => c.GeneralUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Admin>()
                .HasOne(a => a.GeneralUser)
                .WithOne(gu => gu.Admin)
                .HasForeignKey<Admin>(a => a.GeneralUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Individual>()
                .HasOne(i => i.GeneralUser)
                .WithOne(gu => gu.Individual)
                .HasForeignKey<Individual>(i => i.GeneralUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones de Offer
            builder
                .Entity<Offer>()
                .HasOne(o => o.Oferente)
                .WithMany()
                .HasForeignKey(o => o.OferenteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones de JobApplication
            builder
                .Entity<JobApplication>()
                .HasOne(ja => ja.Estudiante)
                .WithMany()
                .HasForeignKey(ja => ja.EstudianteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
