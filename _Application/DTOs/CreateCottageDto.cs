using MediatR; // Jeśli używasz MediatR
using System.ComponentModel.DataAnnotations;

namespace MobileAppCottage._Application.Cottages.Commands.CreateCottage
{
    public class CreateCottageCommand : IRequest // IRequest mówi: "To jest zadanie dla Mediatora"
    {
        [Required(ErrorMessage = "Nazwa domku jest wymagana.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Nazwa musi mieć od 3 do 50 znaków.")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Cena za dobę jest wymagana.")]
        [Range(1, 5000, ErrorMessage = "Cena musi mieścić się w przedziale 1 - 5000 PLN.")]
        public decimal Price { get; set; }

        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public int MaxPersons { get; set; }
    }
}