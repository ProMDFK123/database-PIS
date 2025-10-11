using bolsafeucn_back.src.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<GeneralUser> Usuarios { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Disability> Discapacidades { get; set; }
        public DbSet<Student> Estudiantes { get; set; }
        public DbSet<Individual> Particulares { get; set; }
        public DbSet<Company> Empresas { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
