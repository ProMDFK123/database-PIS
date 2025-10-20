using System.ComponentModel.DataAnnotations;

namespace bolsafeucn_back.src.Application.DTO.PublicationDTO
{
    public class CreateBuySellDTO
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede superar los 100 caracteres.")]
        public required string Title { get; set; }
        
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public required string Description { get; set; }
        
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public required string Category { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        public required decimal Price { get; set; }
        
        public required List<string> ImagesURL { get; set; } = new List<string>();
    }
}
