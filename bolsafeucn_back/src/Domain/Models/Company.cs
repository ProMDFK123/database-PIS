namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Clase que representa una empresa.
    /// </summary>
    public class Company
    {
        public int Id { get; set; }
        public required GeneralUser UsuarioGenerico { get; set; }
        public int UsuarioGenericoId { get; set; }
        public required string Rut { get; set; }
        public required string NombreEmpresa { get; set; }
        public required string RazonSocial { get; set; }
        public float Calificacion { get; set; }
    }
}
