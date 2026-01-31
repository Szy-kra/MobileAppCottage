using System.ComponentModel.DataAnnotations;

namespace MobileAppCottage.Application.DTOs
{
    public class CreateCottageDto
    {
        [Required(ErrorMessage = "Pole 'Nazwa' nie może być puste.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis jest wymagany.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Maksymalna liczba osób jest wymagana.")]
        [Range(1, 20, ErrorMessage = "Pojemność musi mieścić się w przedziale 1-20.")]
        public int MaxPersons { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana.")]
        [Range(0.01, 10000.00, ErrorMessage = "Cena musi być większa od 0.")]
        public decimal Price { get; set; }

        // Dodajemy brakujące pola, które masz w bazie:
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }
}
