using Microsoft.EntityFrameworkCore;
using MobileAppCottage._Application.Mappings;
using MobileAppCottage._Domain.Entities;
using MobileAppCottage.Domain.Entities;
using MobileAppCottage.Domain.Interfaces;
using MobileAppCottage.Infrastructure.Persistence;
using MobileAppCottage.Infrastructure.Repositories;
using NLog;
using NLog.Web;

// 1. Inicjalizacja NLoga
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Services Configuration
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();

    // Konfiguracja bazy danych
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<CottageDbContext>(options =>
        options.UseSqlServer(connectionString));

    // Rejestracja Identity
    builder.Services.AddIdentityApiEndpoints<User>()
        .AddRoles<Role>()
        .AddEntityFrameworkStores<CottageDbContext>();

    // Rejestracja MediatR - Upewnij siê, ¿e CottageMappingProfile istnieje!
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(CottageMappingProfile).Assembly));

    // Rejestracja Repozytorium i Seedera
    builder.Services.AddScoped<ICottageRepository, CottageRepository>();
    builder.Services.AddScoped<CottageSeeder>();

    builder.Services.AddAutoMapper(typeof(CottageMappingProfile));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    #endregion

    // --- KLUCZOWY MOMENT ---
    var app = builder.Build();

    #region Database Seeding
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var seeder = services.GetRequiredService<CottageSeeder>();
        await seeder.Seed();
    }
    #endregion

    #region Middleware Pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapGroup("/identity").MapIdentityApi<User>();
    app.UseAuthorization();
    app.MapControllers();
    #endregion

    logger.Info("Aplikacja uruchomiona pomyœlnie.");
    await app.RunAsync();
}
catch (Exception exception)
{
    // WYJ¥TEK: To wypisze DOK£ADNY b³¹d w czarnym oknie konsoli
    Console.WriteLine("!!! B£¥D KRYTYCZNY STARTU !!!");
    Console.WriteLine(exception.Message);
    if (exception.InnerException != null)
        Console.WriteLine("INNER EXCEPTION: " + exception.InnerException.Message);

    logger.Error(exception, "Program zatrzymany z powodu b³êdu krytycznego.");
    throw;
}
finally
{
    LogManager.Shutdown();
}