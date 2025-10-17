namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Enum que define los tipos de publicaciones en el sistema
    /// </summary>
    public enum Types
    {
        Offer, // Oferta laboral o voluntariado
        Volunteer, // Voluntariado
        BuySell, // Compra/Venta
    }

    /// <summary>
    /// Clase base abstracta para todas las publicaciones del sistema
    /// Hereda de esta clase: Offer, BuySell
    /// </summary>
    public abstract class Publication
    {
        /// <summary>
        /// Identificador único de la publicación
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Usuario que creó la publicación
        /// </summary>
        public required GeneralUser User { get; set; }

        /// <summary>
        /// ID del usuario que creó la publicación
        /// </summary>
        public required int UserId { get; set; }

        /// <summary>
        /// Título de la publicación
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Descripción detallada de la publicación
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Fecha y hora de publicación (UTC)
        /// </summary>
        public DateTime PublicationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Colección de imágenes asociadas a la publicación
        /// </summary>
        public ICollection<Image> Images { get; set; } = new List<Image>();

        /// <summary>
        /// Tipo de publicación (Offer, Volunteer, BuySell)
        /// </summary>
        public required Types Type { get; set; }

        /// <summary>
        /// Indica si la publicación está activa y visible para los usuarios
        /// </summary>
        public bool IsActive { get; set; }
    }
}
