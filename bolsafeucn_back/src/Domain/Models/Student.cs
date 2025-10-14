namespace bolsafeucn_back.src.Domain.Models
{
    public enum Discapacidad
    {
        Ninguna,
        Visual,
        Auditiva,
        Motriz,
        Cognitiva,
        Otra,
    }

    /// <summary>
    /// Identificador único del usuario estudiante.
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        public required GeneralUser UsuarioGenerico { get; set; }
        public required int UsuarioGenericoId { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required Disability Discapacidad { get; set; }
        public float Calificacion { get; set; }
        public string CurriculumVitae { get; set; } = string.Empty;
        public string CartaMotivacional { get; set; } = string.Empty;
    }
}
