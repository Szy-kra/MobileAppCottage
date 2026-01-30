using Microsoft.AspNetCore.Identity; // Do ról i userów
using Microsoft.EntityFrameworkCore;
using MobileAppCottage._Domain.Entities;
using MobileAppCottage.Domain.Entities;

namespace MobileAppCottage.Infrastructure.Persistence
{
    public class CottageSeeder
    {
        private readonly CottageDbContext _dbContext;
        private readonly UserManager<User> _userManager; // Dodane
        private readonly RoleManager<Role> _roleManager; // Dodane
        private readonly ILogger<CottageSeeder> _logger;

        public CottageSeeder(CottageDbContext dbContext,
                             UserManager<User> userManager,
                             RoleManager<Role> roleManager,
                             ILogger<CottageSeeder> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task Seed()
        {
            try
            {
                if (await _dbContext.Database.CanConnectAsync())
                {
                    // 1. DODAJEMY ROLE (Owner, Guest itp.)
                    if (!await _roleManager.Roles.AnyAsync())
                    {
                        _logger.LogInformation("Tworzę role użytkowników.");
                        await _roleManager.CreateAsync(new Role { Name = "Owner" });
                        await _roleManager.CreateAsync(new Role { Name = "Guest" });
                    }

                    // 2. DODAJEMY TESTOWEGO USERA
                    if (!await _userManager.Users.AnyAsync())
                    {
                        var user = new User { UserName = "admin@cottage.pl", Email = "admin@cottage.pl" };
                        await _userManager.CreateAsync(user, "Haslo123!");
                        await _userManager.AddToRoleAsync(user, "Owner");
                    }

                    // 3. DODAJEMY DOMKI
                    if (!await _dbContext.Cottages.AnyAsync())
                    {
                        _logger.LogInformation("Baza danych jest pusta. Dodaję domki.");
                        var owner = await _userManager.FindByEmailAsync("admin@cottage.pl");
                        var initialCottages = GetCottages(owner.Id); // Przekazujemy ID właściciela

                        await _dbContext.Cottages.AddRangeAsync(initialCottages);
                        await _dbContext.SaveChangesAsync();
                        _logger.LogInformation("Pomyślnie dodano dane startowe.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas seedowania.");
            }
        }

        private IEnumerable<Cottage> GetCottages(string ownerId)
        {
            var cottages = new List<Cottage>()
            {
                new Cottage()
                {
                    Name = "Domek pod Lasem",
                    OwnerId = ownerId, // <--- To jest kluczowe!
                    ContactDetails = new CottageDetails() { City = "Zakopane", Price = 350.00m },
                    CreatedAt = DateTime.UtcNow
                }
            };
            foreach (var c in cottages) { c.EncodeName(); }
            return cottages;
        }
    }
}