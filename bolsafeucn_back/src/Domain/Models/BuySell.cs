namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Representa una publicación de compra/venta de productos o servicios
    /// </summary>
    public class BuySell : Publication
    {
        /// <summary>
        /// Precio del producto o servicio en pesos chilenos
        /// </summary>
        public decimal Price { get; set; }
    }
}
