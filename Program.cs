using Microsoft.EntityFrameworkCore;
using MobileAppCottage.API.Middleware;
using MobileAppCottage.Application.Cottages.Commands.CreateCottage; // Dodane, by widzieæ CreateCottageCommand
using MobileAppCottage.Application.Mappings;
using MobileAppCottage.Application.Services;
using MobileAppCottage.Domain.Entities;
using MobileAppCottage.Domain.Interfaces;
using MobileAppCottage.Infrastructure.Persistence;
using MobileAppCottage.Infrastructure.Repositories;
using MobileAppCottage.Infrastructure.UserContext;
using NLog;
using NLog.Web;

// 1. Inicjalizacja NLoga
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Services Configuration
    // Konfiguracja logowania NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();

    // Konfiguracja bazy danych SQL Server
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<CottageDbContext>(options =>
        options.UseSqlServer(connectionString));

    // Rejestracja Identity
    builder.Services.AddIdentityApiEndpoints<User>()
        .AddRoles<Role>()
        .AddEntityFrameworkStores<CottageDbContext>();

    // --- POPRAWIONA REJESTRACJA MEDIATR ---
    // Wskazujemy projekt Application poprzez dowoln¹ klasê, która tam jest
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(CreateCottageCommand).Assembly));

    // Rejestracja us³ug i repozytoriów
    builder.Services.AddScoped<ICottageRepository, CottageRepository>();
    builder.Services.AddScoped<CottageSeeder>();
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<CottageCacheService>();
    builder.Services.AddAutoMapper(typeof(CottageMappingProfile));
    builder.Services.AddScoped<IUserContext, UserContext>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    #endregion

    // --- BUDOWANIE APLIKACJI ---
    var app = builder.Build();

    #region Database Seeding
    if (args.Length == 0 || !args[0].Contains("ef"))
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var seeder = services.GetRequiredService<CottageSeeder>();
                await seeder.Seed();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "B³¹d podczas seedowania bazy danych.");
            }
        }
    }
    #endregion

    #region Middleware Pipeline
    app.UseMiddleware<ErrorHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Endpointy dla logowania i rejestracji (Identity)
    app.MapGroup("/identity").MapIdentityApi<User>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    #endregion

    logger.Info("Aplikacja MobileAppCottage uruchomiona pomyœlnie.");
    await app.RunAsync();
}
catch (Exception exception)
{
    if (exception.GetType().Name == "HostAbortedException")
    {
        throw;
    }

    Console.WriteLine("!!! B£¥D KRYTYCZNY STARTU !!!");
    Console.WriteLine(exception.Message);
    logger.Error(exception, "Program zatrzymany z powodu b³êdu krytycznego podczas startu.");
    throw;
}
finally
{
    LogManager.Shutdown();
}