using Bogus;
using bolsafeucn_back.src.Application.Infrastructure.Data;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Application.Infrastructure.Data
{
    public class DataSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var userManager = scope.ServiceProvider.GetRequiredService<
                    UserManager<GeneralUser>
                >();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                await context.Database.EnsureCreatedAsync();
                await context.Database.MigrateAsync();

                if (!context.Roles.Any())
                {
                    var roles = new List<Role>
                    {
                        new Role { Name = "Admin", NormalizedName = "ADMIN" },
                        new Role { Name = "Applicant", NormalizedName = "APPLICANT" },
                        new Role { Name = "Offerent", NormalizedName = "OFFERENT" },
                        new Role { Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                    };
                    foreach (var role in roles)
                    {
                        var result = roleManager.CreateAsync(role).GetAwaiter().GetResult();
                        if (!result.Succeeded)
                        {
                            throw new Exception($"No se pudo crear el rol {role.Name}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al inicializar la base de datos: {ex.Message}", ex);
            }
        }
    }
}
