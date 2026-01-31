using MediatR;

namespace MobileAppCottage.Application.Cottages.Commands.CreateCottage
{
    // Zwracamy int (Id nowego domku)
    public class CreateCottageCommand : IRequest<int>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; } // Mapujemy do ContactDetails.Description
        public decimal Price { get; set; }        // Cena za dobę
        public int MaxPersons { get; set; }       // Ilość osób
        public string? City { get; set; }         // Lokalizacja

        // Dodatkowe opcjonalne pola z WebApp
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }
}