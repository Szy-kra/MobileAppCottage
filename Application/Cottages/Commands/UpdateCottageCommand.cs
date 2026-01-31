using MediatR;

namespace MobileAppCottage.Application.Cottages.Commands.UpdateCottage
{
    public class UpdateCottageCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public double Price { get; set; }
        public int MaxPersons { get; set; }
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
    }
}