using Microsoft.EntityFrameworkCore;
using MobileAppCottage.Domain.Entities;
using MobileAppCottage.Domain.Interfaces;
using MobileAppCottage.Infrastructure.Persistence;


namespace MobileAppCottage.Infrastructure.Repositories;

public class CottageRepository : ICottageRepository
{
    private readonly CottageDbContext _context;
    private readonly ILogger<CottageRepository> _logger; // Nasz łącznik z NLogiem

    // Dodajemy logger do konstruktora
    public CottageRepository(CottageDbContext context, ILogger<CottageRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> AddAsync(Cottage cottage)
    {
        // Logujemy próbę dodania domku
        _logger.LogInformation("Próba dodania nowego domku o nazwie: {CottageName}", cottage.Name);

        _context.Cottages.Add(cottage);
        await _context.SaveChangesAsync();

        // Logujemy sukces
        _logger.LogInformation("Pomyślnie dodano domek. Nadane ID: {CottageId}", cottage.Id);

        return cottage.Id;
    }

    public async Task<IEnumerable<Cottage>> GetAllAsync()
    {
        _logger.LogInformation("Pobieranie listy wszystkich domków z bazy.");
        return await _context.Cottages.ToListAsync();
    }
}