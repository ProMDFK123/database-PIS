using System.ComponentModel.DataAnnotations;

namespace bolsafeucn_back.src.Application.DTO.PublicationDTO
{
    public class CreateOfferDTO : IValidatableObject
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede superar los 100 caracteres.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public string Category { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "La fecha de expiración es obligatoria.")]
        public DateTime ExpirationDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La remuneración debe ser mayor o igual a 0.")]
        public decimal Remuneration { get; set; }

        public List<string> ImagesURL { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExpirationDate <= PublicationDate)
            {
                yield return new ValidationResult(
                    "La fecha de expiración debe ser posterior a la fecha de publicación.",
                    new[] { nameof(ExpirationDate) }
                );
            }
        }
    }
}
