using MobileAppCottage.Domain.Entities;

namespace MobileAppCottage.Domain.Interfaces;

public interface ICottageRepository
{
    Task<int> AddAsync(Cottage cottage);
    Task<IEnumerable<Cottage>> GetAllAsync();
}