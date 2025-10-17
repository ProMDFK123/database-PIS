namespace bolsafeucn_back.src.Domain.Models
{
    public enum Tipos
    {
        Trabajo,
        Voluntariado,
        CompraVenta,
    }

    public class Offer : Publication
    {
        public DateTime FechaFin { get; set; }
        public DateTime FechaLimite { get; set; }
        public required int Remuneracion { get; set; }
        public required Tipos Tipo { get; set; }
        public bool Activa { get; set; }
    }
}
