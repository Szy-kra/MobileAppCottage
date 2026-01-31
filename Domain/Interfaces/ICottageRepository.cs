using MobileAppCottage.Domain.Entities;

namespace MobileAppCottage.Domain.Interfaces
{
    public interface ICottageRepository
    {
        // Twoje działające metody
        Task<int> Create(Cottage cottage);
        Task<IEnumerable<Cottage>> GetAll();
        Task<Cottage?> GetById(int id);
        Task<Cottage?> GetByEncodedName(string encodedName);

        // Poprawione metody Update i Delete pod Handlery MediatR
        Task Update(int id, Cottage cottage);
        Task Delete(int id);
    }
}