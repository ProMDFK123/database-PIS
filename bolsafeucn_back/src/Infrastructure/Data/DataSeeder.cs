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
            var faker = new Faker("es");

            // ========================================
            // USUARIOS DE PRUEBA CON CREDENCIALES FÁCILES
            // ========================================
            Log.Information("DataSeeder: Creando usuarios de prueba con credenciales fáciles...");

            // 1. ESTUDIANTE DE PRUEBA
            var testStudentUser = new GeneralUser
            {
                UserName = "estudiante",
                Email = "estudiante@alumnos.ucn.cl",
                PhoneNumber = "+56912345678",
                UserType = UserType.Estudiante,
                Rut = "12345678-9",
                EmailConfirmed = true,
                Banned = false,
            };
            var studentResult = await userManager.CreateAsync(testStudentUser, "Test123!");
            if (studentResult.Succeeded)
            {
                await userManager.AddToRoleAsync(testStudentUser, "Applicant");
                var testStudent = new Student
                {
                    GeneralUserId = testStudentUser.Id,
                    Name = "Juan",
                    LastName = "Pérez Estudiante",
                    Disability = Disability.Ninguna,
                    GeneralUser = testStudentUser,
                    CurriculumVitae = "https://ejemplo.com/cv/juan_perez.pdf", // CV de prueba
                    MotivationLetter = "Soy un estudiante motivado y con ganas de aprender", // Carta opcional
                };
                context.Students.Add(testStudent);
                Log.Information(
                    "✅ Usuario estudiante creado: estudiante@alumnos.ucn.cl / Test123!"
                );
            }

            // 2. EMPRESA DE PRUEBA
            var testCompanyUser = new GeneralUser
            {
                UserName = "empresa",
                Email = "empresa@techcorp.cl",
                PhoneNumber = "+56987654321",
                UserType = UserType.Empresa,
                Rut = "76543210-K",
                EmailConfirmed = true,
                Banned = false,
            };
            var companyResult = await userManager.CreateAsync(testCompanyUser, "Test123!");
            if (companyResult.Succeeded)
            {
                await userManager.AddToRoleAsync(testCompanyUser, "Offerent");
                var testCompany = new Company
                {
                    GeneralUserId = testCompanyUser.Id,
                    CompanyName = "Tech Corp SpA",
                    LegalName = "Tecnología Corporativa SpA",
                    GeneralUser = testCompanyUser,
                };
                context.Companies.Add(testCompany);
                Log.Information("✅ Usuario empresa creado: empresa@techcorp.cl / Test123!");
            }

            // 3. PARTICULAR DE PRUEBA
            var testIndividualUser = new GeneralUser
            {
                UserName = "particular",
                Email = "particular@ucn.cl",
                PhoneNumber = "+56955555555",
                UserType = UserType.Particular,
                Rut = "11222333-4",
                EmailConfirmed = true,
                Banned = false,
            };
            var individualResult = await userManager.CreateAsync(testIndividualUser, "Test123!");
            if (individualResult.Succeeded)
            {
                await userManager.AddToRoleAsync(testIndividualUser, "Offerent");
                var testIndividual = new Individual
                {
                    GeneralUserId = testIndividualUser.Id,
                    Name = "María",
                    LastName = "González Particular",
                    GeneralUser = testIndividualUser,
                };
                context.Individuals.Add(testIndividual);
                Log.Information("✅ Usuario particular creado: particular@ucn.cl / Test123!");
            }

            // 4. ADMIN DE PRUEBA
            var testAdminUser = new GeneralUser
            {
                UserName = "admin",
                Email = "admin@ucn.cl",
                PhoneNumber = "+56911111111",
                UserType = UserType.Administrador,
                Rut = "99888777-6",
                EmailConfirmed = true,
                Banned = false,
            };
            var adminResult = await userManager.CreateAsync(testAdminUser, "Test123!");
            if (adminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(testAdminUser, "Admin");
                var testAdmin = new Admin
                {
                    GeneralUserId = testAdminUser.Id,
                    Name = "Carlos",
                    LastName = "Admin Sistema",
                    SuperAdmin = false,
                    GeneralUser = testAdminUser,
                };
                context.Admins.Add(testAdmin);
                Log.Information("✅ Usuario admin creado: admin@ucn.cl / Test123!");
            }

            Log.Information("DataSeeder: Usuarios de prueba creados exitosamente.");
            Log.Information("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Log.Information("📋 CREDENCIALES DE PRUEBA:");
            Log.Information("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Log.Information("👨‍🎓 ESTUDIANTE:");
            Log.Information("   Email: estudiante@alumnos.ucn.cl");
            Log.Information("   Pass:  Test123!");
            Log.Information("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Log.Information("🏢 EMPRESA:");
            Log.Information("   Email: empresa@techcorp.cl");
            Log.Information("   Pass:  Test123!");
            Log.Information("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Log.Information("👤 PARTICULAR:");
            Log.Information("   Email: particular@ucn.cl");
            Log.Information("   Pass:  Test123!");
            Log.Information("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Log.Information("👑 ADMIN:");
            Log.Information("   Email: admin@ucn.cl");
            Log.Information("   Pass:  Test123!");
            Log.Information("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

            // ========================================
            // USUARIOS ALEATORIOS ADICIONALES (Faker)
            // ========================================
            Log.Information("DataSeeder: Creando usuarios aleatorios adicionales...");

            // Seed Random Students
            for (int i = 0; i < 3; i++)
            {
                var studentUser = new GeneralUser
                {
                    UserName = faker.Internet.UserName(),
                    Email = faker.Internet.Email(),
                    PhoneNumber = faker.Phone.PhoneNumber("+569########"),
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
            var randomIndividualUser = new GeneralUser
            {
                UserName = faker.Internet.UserName(),
                Email = faker.Internet.Email(),
                UserType = UserType.Particular,
                Rut = faker.Random.Replace("##.###.###-K"),
                EmailConfirmed = true,
                Banned = false,
            };
            var randomIndividualResult = await userManager.CreateAsync(
                randomIndividualUser,
                "Password123!"
            );
            if (randomIndividualResult.Succeeded)
            {
                await userManager.AddToRoleAsync(randomIndividualUser, "Offerent");
                var randomIndividual = new Individual
                {
                    GeneralUserId = randomIndividualUser.Id,
                    Name = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    GeneralUser = randomIndividualUser,
                };
                context.Individuals.Add(randomIndividual);
            }

            await context.SaveChangesAsync();
            Log.Information("DataSeeder: Todos los usuarios creados exitosamente.");
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
                    // Propiedades heredadas de Publication (clase base)
                    UserId = selectedOfferent.Id,
                    User = selectedOfferent,
                    Title = faker.Lorem.Sentence(3),
                    Description = faker.Lorem.Paragraph(),
                    PublicationDate = DateTime.UtcNow,
                    Type = Types.Offer,
                    IsActive = true,

                    // Propiedades específicas de Offer
                    EndDate = DateTime.UtcNow.AddDays(faker.Random.Int(10, 30)),
                    DeadlineDate = DateTime.UtcNow.AddDays(faker.Random.Int(5, 9)),
                    Remuneration = faker.Random.Int(500, 2000),
                    OfferType = faker.PickRandom<OfferTypes>(),
                    Location = faker.Address.City(),
                    Requirements = faker.Lorem.Sentence(),
                    ContactInfo = faker.Internet.Email(),
                };
                context.Offers.Add(offer);
            }
            await context.SaveChangesAsync();
        }
    }
}
