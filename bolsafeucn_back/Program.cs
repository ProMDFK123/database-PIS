using bolsafeucn_back.src.Application.Infrastructure.Data;
using bolsafeucn_back.src.Application.Mappers;
using bolsafeucn_back.src.Application.Services.Implements;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Implements;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resend;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configuracion de Identity
builder
    .Services.AddIdentity<GeneralUser, Role>(options =>
    {
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region Configuracion de autenticacion y autorizacion
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        string? jwtSecret = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new InvalidOperationException("La clave secreta JWT no está configurada.");
        }
        options.TokenValidationParameters =
            new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(jwtSecret)
                ),
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            };
    });
#endregion

#region Configuracion de Resend
builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(o =>
{
    o.ApiToken = builder.Configuration.GetValue<string>("ResendApiKey")!;
});
builder.Services.AddTransient<IResend, ResendClient>();
#endregion

#region Configuracion de PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#endregion

#region Injeccion de dependencias
builder.Services.AddScoped<StudentMapper>();
builder.Services.AddScoped<IndividualMapper>();
builder.Services.AddScoped<CompanyMapper>();
builder.Services.AddScoped<AdminMapper>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
#endregion

var app = builder.Build();

#region Inicializacion de la base de datos y mapeos
using (var scope = app.Services.CreateScope())
{
    await DataSeeder.Initialize(app.Services);

    MapperExtensions.ConfigureMapster(scope.ServiceProvider);
}
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
