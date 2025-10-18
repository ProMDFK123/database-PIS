namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs
{
    public class OfferDetailsDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty; // Cargo
        public string Descripcion { get; set; } = string.Empty;
        public string Requisitos { get; set; } = string.Empty;
        public string Duracion { get; set; } = string.Empty;
        public string CorreoContacto { get; set; } = string.Empty;
        public string TelefonoContacto { get; set; } = string.Empty;
        public bool Activa { get; set; } // Indica si está disponible para postular
    }
}
