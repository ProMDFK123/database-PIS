using Microsoft.AspNetCore.Identity;

namespace bolsafeucn_back.src.Domain.Models
{
    public class GeneralUser : IdentityUser
    {
        /// <summary>
        /// Identificador único del usuario con sus atributos compartidos entre todos los roles.
        /// </summary>
        public required string Correo { get; set; }
        public required string Telefono { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }
}
