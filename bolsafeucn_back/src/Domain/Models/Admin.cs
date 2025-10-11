namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Clase que representa un administrador.
    /// </summary>
    public class Admin
    {
        public int Id { get; set; }
        public required GeneralUser UsuarioGenerico { get; set; }
        public required int UsuarioGenericoId { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Rut { get; set; }
        public bool SuperAdmin { get; set; } = false;
    }
}
