using Microsoft.AspNetCore.Identity;

namespace bolsafeucn_back.src.Domain.Models
{
    public enum UserType
    {
        Estudiante,
        Empresa,
        Particular,
        Administrador,
    }

    public class GeneralUser : IdentityUser<int>
    {
        public required UserType UserType { get; set; }
        public required string Rut { get; set; }
        public required bool Banned { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Publication> Publications { get; set; } = new List<Publication>();

        //Coneccion con los tipos de usuario
        public Student? Student { get; set; }
        public Company? Company { get; set; }
        public Individual? Individual { get; set; }
        public Admin? Admin { get; set; }
    }
}