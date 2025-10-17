namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Enum que define los tipos de ofertas disponibles en el sistema
    /// </summary>
    public enum OfferTypes
    {
        Trabajo,
        Voluntariado,
    }

    /// <summary>
    /// Representa una oferta laboral o voluntariado publicada en el sistema
    /// </summary>
    public class Offer : Publication
    {
        /// <summary>
        /// Fecha de finalización de la oferta
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Fecha límite para postular a la oferta
        /// </summary>
        public DateTime DeadlineDate { get; set; }

        /// <summary>
        /// Remuneración ofrecida (en pesos chilenos). 0 para voluntariados
        /// </summary>
        public required int Remuneration { get; set; }

        /// <summary>
        /// Tipo de oferta (Trabajo, Voluntariado)
        /// </summary>
        public required OfferTypes OfferType { get; set; }

        /// <summary>
        /// Ubicación del trabajo (ciudad, región, remoto, etc.)
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Requisitos específicos para la oferta
        /// </summary>
        public string? Requirements { get; set; }

        /// <summary>
        /// Información de contacto (email o teléfono)
        /// </summary>
        public string? ContactInfo { get; set; }
    }
}
