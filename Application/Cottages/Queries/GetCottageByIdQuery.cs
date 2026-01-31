using MediatR;
using MobileAppCottage.Application.DTOs;

namespace MobileAppCottage.Application.Cottages.Queries
{
    // Zapytanie o konkretny domek po jego ID
    public record GetCottageByIdQuery(int Id) : IRequest<CottageDto>;
}