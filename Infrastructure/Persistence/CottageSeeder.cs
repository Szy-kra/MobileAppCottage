using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MobileAppCottage.Domain.Entities;

namespace MobileAppCottage.Infrastructure.Persistence
{
    public class CottageSeeder
    {
        private readonly CottageDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<CottageSeeder> _logger;

        // Stałe ID dla admina, żebyś mógł je łatwo wkleić do Postmana
        private const string AdminId = "8e445865-a24d-4543-a6c6-9443d048cdb9";

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
                    // 1. DODAJEMY ROLE
                    if (!await _roleManager.Roles.AnyAsync())
                    {
                        _logger.LogInformation("Tworzę role: Owner, Guest.");
                        await _roleManager.CreateAsync(new Role { Name = "Owner" });
                        await _roleManager.CreateAsync(new Role { Name = "Guest" });
                    }

                    // 2. DODAJEMY TESTOWEGO UŻYTKOWNIKA (jeśli nie istnieje)
                    var adminUser = await _userManager.FindByIdAsync(AdminId);
                    if (adminUser == null)
                    {
                        _logger.LogInformation("Tworzę testowego użytkownika Admin.");
                        var user = new User
                        {
                            Id = AdminId, // Przypisujemy stałe ID!
                            UserName = "admin@cottage.pl",
                            Email = "admin@cottage.pl",
                            FirstName = "Admin",
                            LastName = "Cottage"
                        };

                        var result = await _userManager.CreateAsync(user, "Haslo123!");
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Owner");
                        }
                    }

                    // 3. DODAJEMY DOMKI STARTOWE
                    if (!await _dbContext.Cottages.AnyAsync())
                    {
                        _logger.LogInformation("Dodaję przykładowe domki do bazy.");
                        var initialCottages = GetCottages(AdminId);

                        await _dbContext.Cottages.AddRangeAsync(initialCottages);
                        await _dbContext.SaveChangesAsync();
                        _logger.LogInformation("Pomyślnie zasiedlono bazę danych.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas seedowania danych.");
            }
        }

        private IEnumerable<Cottage> GetCottages(string ownerId)
        {
            var cottages = new List<Cottage>()
            {
                new Cottage()
                {
                    Name = "Domek pod Lasem",
                    Description = "Pierwszy testowy opis domku",
                    OwnerId = ownerId,
                    ContactDetails = new CottageDetails()
                    {
                        City = "Zakopane",
                        Price = 350.00m,
                        Description = "Dodatkowy opis kontaktu"
                    },
                    CreatedAt = DateTime.UtcNow
                }
            };

            foreach (var c in cottages)
            {
                c.EncodeName();
            }
            return cottages;
        }
    }
}