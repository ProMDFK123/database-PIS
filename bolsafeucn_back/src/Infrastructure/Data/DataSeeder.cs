using Bogus;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace bolsafeucn_back.src.Application.Infrastructure.Data
{
    public class DataSeeder
    {
        public static async Task Initialize(
            IConfiguration configuration,
            IServiceProvider serviceProvider
        )
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            try
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<GeneralUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

                Log.Information("DataSeeder: Iniciando la migración de la base de datos...");
                await context.Database.MigrateAsync();
                Log.Information("DataSeeder: Migración de la base de datos completada.");

                if (!await context.Roles.AnyAsync())
                {
                    Log.Information("DataSeeder: No se encontraron roles, creando roles...");
                    var roles = new List<Role>
                    {
                        new Role { Name = "Admin", NormalizedName = "ADMIN" },
                        new Role { Name = "Applicant", NormalizedName = "APPLICANT" },
                        new Role { Name = "Offerent", NormalizedName = "OFFERENT" },
                        new Role { Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                    };
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }
                    Log.Information("DataSeeder: Roles creados exitosamente.");
                }

                if (!await context.Users.AnyAsync())
                {
                    Log.Information(
                        "DataSeeder: No se encontraron usuarios, creando usuarios de prueba..."
                    );
                    await SeedUsers(userManager, context);
                    Log.Information("DataSeeder: Usuarios de prueba creados exitosamente.");
                }

                if (!await context.Offers.AnyAsync())
                {
                    Log.Information(
                        "DataSeeder: No se encontraron ofertas, creando ofertas de prueba..."
                    );
                    await SeedOffers(context);
                    Log.Information("DataSeeder: Ofertas de prueba creadas exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "DataSeeder: Error al inicializar la base de datos.");
                throw;
            }
        }

        private static async Task SeedUsers(
            UserManager<GeneralUser> userManager,
            AppDbContext context
        )
        {
            // ... (El resto del método SeedUsers se mantiene igual)
            var faker = new Faker("es");

            // Seed Students
            for (int i = 0; i < 5; i++)
            {
                var studentUser = new GeneralUser
                {
                    UserName = faker.Internet.UserName(),
                    Email = faker.Internet.Email(),
                    UserType = UserType.Estudiante,
                    Rut = faker.Random.Replace("##.###.###-K"),
                    EmailConfirmed = true,
                    Banned = false,
                };
                var result = await userManager.CreateAsync(studentUser, "Password123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(studentUser, "Applicant");
                    var student = new Student
                    {
                        GeneralUserId = studentUser.Id,
                        Name = faker.Name.FirstName(),
                        LastName = faker.Name.LastName(),
                        Disability = Disability.Ninguna,
                        GeneralUser = studentUser,
                    };
                    context.Students.Add(student);
                }
            }

            // Seed Companies
            for (int i = 0; i < 2; i++)
            {
                var companyUser = new GeneralUser
                {
                    UserName = faker.Internet.UserName(),
                    Email = faker.Internet.Email(),
                    UserType = UserType.Empresa,
                    Rut = faker.Random.Replace("##.###.###-K"),
                    EmailConfirmed = true,
                    Banned = false,
                };
                var result = await userManager.CreateAsync(companyUser, "Password123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(companyUser, "Offerent");
                    var company = new Company
                    {
                        GeneralUserId = companyUser.Id,
                        CompanyName = faker.Company.CompanyName(),
                        LegalName = faker.Company.CompanyName() + " S.A.",
                        GeneralUser = companyUser,
                    };
                    context.Companies.Add(company);
                }
            }

            // Seed Individual
            var individualUser = new GeneralUser
            {
                UserName = faker.Internet.UserName(),
                Email = faker.Internet.Email(),
                UserType = UserType.Particular,
                Rut = faker.Random.Replace("##.###.###-K"),
                EmailConfirmed = true,
                Banned = false,
            };
            var individualResult = await userManager.CreateAsync(individualUser, "Password123!");
            if (individualResult.Succeeded)
            {
                await userManager.AddToRoleAsync(individualUser, "Offerent");
                var individual = new Individual
                {
                    GeneralUserId = individualUser.Id,
                    Name = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    GeneralUser = individualUser,
                };
                context.Individuals.Add(individual);
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedOffers(AppDbContext context)
        {
            var faker = new Faker("es");
            var offerents = await context
                .Users.Where(u =>
                    u.UserType == UserType.Empresa || u.UserType == UserType.Particular
                )
                .ToListAsync();

            if (offerents.Count == 0)
                return;

            for (int i = 0; i < 10; i++)
            {
                var selectedOfferent = faker.PickRandom(offerents);
                var offer = new Offer
                {
                    // Propiedades de Publication (clase base)
                    UserId = selectedOfferent.Id,
                    User = selectedOfferent,
                    Title = faker.Lorem.Sentence(3),
                    Description = faker.Lorem.Paragraph(),
                    PublicationDate = DateTime.UtcNow,
                    Type = Types.Offer,
                    IsActive = true,

                    // Propiedades específicas de Offer
                    FechaFin = DateTime.UtcNow.AddDays(faker.Random.Int(10, 30)),
                    FechaLimite = DateTime.UtcNow.AddDays(faker.Random.Int(5, 9)),
                    Remuneracion = faker.Random.Int(500, 2000),
                    Tipo = faker.PickRandom<Tipos>(),
                    Activa = true,
                };
                context.Offers.Add(offer);
            }
            await context.SaveChangesAsync();
        }
    }
}
