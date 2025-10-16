namespace bolsafeucn_back.src.Domain.Models
{
    public enum Tipos
    {
        Trabajo,
        Voluntariado,
        CompraVenta,
    }

    public class Offer
    {
        public int Id { get; set; }
        public required GeneralUser Oferente { get; set; }
        public required int OferenteId { get; set; }
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaFin { get; set; }
        public DateTime FechaLimite { get; set; }
        public ICollection<Image> Imagenes { get; set; } = new List<Image>();
        public required int Remuneracion { get; set; }
        public required Tipos Tipo { get; set; }
        public bool Activa { get; set; }
    }
}
