using System.ComponentModel.DataAnnotations; // 1. TO MUSI TU BYĆ

namespace MobileAppCottage._Application.DTOs
{
    public class CreateCottageDto
    {
        [Required(ErrorMessage = "Pole 'Nazwa' nie może być puste.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis jest wymagany.")]
        public string Description { get; set; } = string.Empty;

        [Range(1, 20, ErrorMessage = "Pojemność musi mieścić się w przedziale 1-20.")]
        public int Capacity { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal PricePerNight { get; set; }
    }
}