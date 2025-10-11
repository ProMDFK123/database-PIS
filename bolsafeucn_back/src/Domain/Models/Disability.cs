namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Clase que representa una discapacidad.
    /// </summary>
    public class Disability
    {
        public int Id { get; set; }
        public required string TipoDiscapacidad { get; set; }
    }
}
