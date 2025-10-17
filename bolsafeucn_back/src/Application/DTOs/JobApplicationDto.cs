namespace bolsafeucn_back.src.Application.DTOs
{
    public class CreateJobApplicationDto
    {
        public int OfertaLaboralId { get; set; }
        public string? CartaMotivacional { get; set; }
    }

    public class JobApplicationResponseDto
    {
        public int Id { get; set; }
        public string EstudianteNombre { get; set; } = string.Empty;
        public string EstudianteCorreo { get; set; } = string.Empty;
        public string OfertaTitulo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaPostulacion { get; set; }
        public string? CurriculumVitae { get; set; }
        public string? CartaMotivacional { get; set; }
    }
}