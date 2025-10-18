namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs
{
    /// <summary>
    /// DTO para información básica de ofertas (sin información de contacto)
    /// Se usa para usuarios no-estudiantes para proteger información sensible
    /// </summary>
    public class OfferBasicDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public int Remuneracion { get; set; }
        public string TipoOferta { get; set; } = string.Empty;
        public DateTime FechaPublicacion { get; set; }
        public DateTime FechaTermino { get; set; }
        public bool Activa { get; set; }

        // NOTA: No incluye ContactInfo ni Requirements para proteger datos sensibles
    }
}
