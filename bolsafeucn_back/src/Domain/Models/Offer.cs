namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Enum que define los tipos de ofertas disponibles en el sistema
    /// </summary>
    public enum OfferTypes
    {
        Trabajo,
        Voluntariado,
        CompraVenta,
    }

    /// <summary>
    /// Representa una oferta laboral, voluntariado o compra/venta publicada en el sistema
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
        /// Remuneración ofrecida (en pesos chilenos)
        /// </summary>
        public required int Remuneration { get; set; }

        /// <summary>
        /// Tipo de oferta (Trabajo, Voluntariado, CompraVenta)
        /// </summary>
        public required OfferTypes OfferType { get; set; }

        /// <summary>
        /// Indica si la oferta está activa y visible
        /// </summary>
        public bool Active { get; set; }
    }
}
