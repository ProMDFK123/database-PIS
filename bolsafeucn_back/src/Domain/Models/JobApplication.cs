namespace bolsafeucn_back.src.Domain.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public required GeneralUser Estudiante { get; set; }
        public required int EstudianteId { get; set; }
        public required Offer OfertaLaboral { get; set; }
        public required int OfertaLaboralId { get; set; }
        public required string Estado { get; set; }
        public DateTime FechaPostulacion { get; set; } = DateTime.UtcNow;
    }
}
