using bolsafeucn.src.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Tipos de usuario en el sistema.
    /// </summary>
    public enum UserType
    {
        Student,
        Company,
        Individual,
        Admin,
    }

    public class GeneralUser
    {
        /// <summary>
        /// Identificador único del usuario con los atributos compartidos entre todos los tipos.
        /// </summary>
        public int Id { get; set; }
        public required int IdUsuario { get; set; }
        public required UserType TipoUsuario { get; set; }
        public required string Correo { get; set; }
        public required string Telefono { get; set; }
        public required bool Bloqueado { get; set; } = false;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        //Coneccion con los tipos de usuario
        public ApplicationUser? Usuario { get; set; }
        public Student? Estudiante { get; set; }
        public Company? Empresa { get; set; }
        public Individual? Individual { get; set; }
        public Admin? Administrador { get; set; }
    }
}
