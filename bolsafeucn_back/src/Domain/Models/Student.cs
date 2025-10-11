namespace bolsafeucn_back.src.Domain.Models
{
    public class Student
    {
        /// <summary>
        /// Identificador único del usuario estudiante.
        /// </summary>
        public int Id { get; set; }
        public required GeneralUser UsuarioGenerico { get; set; }
        public required int UsuarioGenericoId { get; set; }
        public required string Rut { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required Disability Discapacidad { get; set; }
        public int DiscapacidadId { get; set; }
        public float Calificacion { get; set; }
        public string CurriculumVitae { get; set; } = string.Empty;
        public string CartaMotivacional { get; set; } = string.Empty;
    }
}
