namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Representa una postulación de un estudiante a una oferta laboral
    /// </summary>
    public class JobApplication
    {
        /// <summary>
        /// Identificador único de la postulación
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Estudiante que realiza la postulación
        /// </summary>
        public required GeneralUser Student { get; set; }

        /// <summary>
        /// ID del estudiante que postula
        /// </summary>
        public required int StudentId { get; set; }

        /// <summary>
        /// Oferta laboral a la que se postula
        /// </summary>
        public required Offer JobOffer { get; set; }

        /// <summary>
        /// ID de la oferta laboral
        /// </summary>
        public required int JobOfferId { get; set; }

        /// <summary>
        /// Estado de la postulación (Pendiente, Aceptada, Rechazada, etc.)
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// Fecha y hora en que se realizó la postulación (UTC)
        /// </summary>
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    }
}
