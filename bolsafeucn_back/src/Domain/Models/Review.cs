namespace bolsafeucn_back.src.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }
        public required float Calificacion { get; set; }
        public required string Comentario { get; set; }
        public GeneralUser? Estudiante { get; set; }
        public string? EstudianteId { get; set; }
        public GeneralUser? Oferente { get; set; }
        public string? OferenteId { get; set; }
    }
}
