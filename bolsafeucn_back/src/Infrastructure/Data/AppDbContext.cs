using bolsafeucn_back.src.Domain.Models;
using bolsafeucn.src.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<Offer> Offers { get; set; } // Nota: el nombre debe ser plural (Offers)

        public DbSet<GeneralUser> Usuarios { get; set; }
        public DbSet<Disability> Discapacidades { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Student> Estudiantes { get; set; }
        public DbSet<Company> Empresas { get; set; }
        public DbSet<Individual> Particulares { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public object Offer { get; internal set; }

        //public DbSet<Offer> Ofertas { get; set; }
        //public DbSet<JobApplication> Postulaciones { get; set; }
        //public DbSet<Review> Evaluaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder
                .Entity<GeneralUser>()
                .HasOne(u => u.Usuario)
                .WithOne(u => u.GeneralUser)
                .HasForeignKey<GeneralUser>(u => u.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Student>()
                .HasOne(s => s.UsuarioGenerico)
                .WithOne()
                .HasForeignKey<Student>(s => s.UsuarioGenericoId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Company>()
                .HasOne(c => c.UsuarioGenerico)
                .WithOne()
                .HasForeignKey<Company>(c => c.UsuarioGenericoId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Admin>()
                .HasOne(a => a.UsuarioGenerico)
                .WithOne()
                .HasForeignKey<Admin>(a => a.UsuarioGenericoId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Individual>()
                .HasOne(i => i.UsuarioGenerico)
                .WithOne()
                .HasForeignKey<Individual>(i => i.UsuarioGenericoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
