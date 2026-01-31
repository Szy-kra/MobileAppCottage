using Microsoft.EntityFrameworkCore;
using MobileAppCottage.API.Middleware;
using MobileAppCottage.Application.Cottages.Commands.CreateCottage;
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

    // --- POPRAWKA: ODPORNOŒÆ NA B£ÊDY SQL (Retry Logic) ---
    // Dodajemy EnableRetryOnFailure, bo baza w Dockerze wstaje wolniej ni¿ API
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<CottageDbContext>(options =>
        options.UseSqlServer(connectionString, sqlOptions =>
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)));

    // Rejestracja Identity
    builder.Services.AddIdentityApiEndpoints<User>()
        .AddRoles<Role>()
        .AddEntityFrameworkStores<CottageDbContext>();

    // Rejestracja MediatR
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

    var app = builder.Build();

    #region Database Initialization & Seeding
    // --- POPRAWKA: TWORZENIE BAZY W DOCKERZE ---
    // Musimy wymusiæ stworzenie bazy, zanim Seeder do niej uderzy [cite: 2026-01-14]
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<CottageDbContext>();

            // Jeœli baza nie istnieje (np. po raz pierwszy w Dockerze), stwórz j¹ [cite: 2026-01-14]
            if (app.Environment.IsDevelopment())
            {
                // EnsureCreated() stworzy bazê bez migracji, co jest szybsze do testów Dockerowych [cite: 2026-01-14]
                await dbContext.Database.EnsureCreatedAsync();
            }

            var seeder = services.GetRequiredService<CottageSeeder>();
            await seeder.Seed();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "B³¹d podczas inicjalizacji lub seedowania bazy danych.");
            // Nie rzucamy wyj¹tku dalej, by aplikacja mog³a spróbowaæ wstaæ mimo b³êdu bazy [cite: 2026-01-14]
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

    // --- UWAGA NA DOCKERA ---
    // W Dockerze czêsto wy³¹cza siê HttpsRedirection, jeœli nie masz skonfigurowanych certyfikatów
    // app.UseHttpsRedirection(); 

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