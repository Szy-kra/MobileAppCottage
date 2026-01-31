using Microsoft.EntityFrameworkCore;
using MobileAppCottage.Domain.Entities;
using MobileAppCottage.Domain.Interfaces;
using MobileAppCottage.Infrastructure.Persistence;

namespace MobileAppCottage.Infrastructure.Repositories
{
    public class CottageRepository : ICottageRepository
    {
        private readonly CottageDbContext _context;

        public CottageRepository(CottageDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Cottage cottage)
        {
            _context.Cottages.Add(cottage);
            await _context.SaveChangesAsync();
            return cottage.Id;
        }

        public async Task<IEnumerable<Cottage>> GetAll()
            => await _context.Cottages
                .Include(c => c.ContactDetails)
                .ToListAsync();

        public async Task<Cottage?> GetById(int id)
            => await _context.Cottages
                .Include(c => c.ContactDetails)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Cottage?> GetByEncodedName(string encodedName)
            => await _context.Cottages
                .Include(c => c.ContactDetails)
                .FirstOrDefaultAsync(c => c.EncodedName == encodedName);

        public async Task Update(int id, Cottage cottage)
        {
            var existing = await GetById(id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(cottage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var cottage = await GetById(id);
            if (cottage != null)
            {
                _context.Cottages.Remove(cottage);
                await _context.SaveChangesAsync();
            }
        }
    }
}