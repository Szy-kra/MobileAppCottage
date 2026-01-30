using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MobileAppCottage._Domain.Entities;
using MobileAppCottage.Domain.Entities;

namespace MobileAppCottage.Infrastructure.Persistence
{
    public class CottageDbContext : IdentityDbContext<User, Role, string>
    {
        public CottageDbContext(DbContextOptions<CottageDbContext> options) : base(options)
        {
        }

        public DbSet<Cottage> Cottages { get; set; }
        public DbSet<CottageImage> CottageImages { get; set; }
        public DbSet<CottageReservation> CottageReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Musi być pierwsze dla Identity
            base.OnModelCreating(modelBuilder);

            // 2. KONFIGURACJA COTTAGE
            modelBuilder.Entity<Cottage>(eb =>
            {
                eb.Property(c => c.Name).IsRequired().HasMaxLength(50);

                // Konfiguracja spłaszczonych danych (Owned Type) i ceny
                eb.OwnsOne(c => c.ContactDetails, cd =>
                {
                    // POPRAWKA: Ustawienie precyzji do 2 miejsc po przecinku (grosze)
                    cd.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");
                });

                eb.HasOne(c => c.Owner)
                  .WithMany(u => u.OwnedCottages)
                  .HasForeignKey(c => c.OwnerId);
            });

            // 3. KONFIGURACJA OBRAZÓW
            modelBuilder.Entity<CottageImage>()
                .Property(i => i.Url).IsRequired();

            // 4. KONFIGURACJA REZERWACJI
            modelBuilder.Entity<CottageReservation>(eb =>
            {
                eb.Property(r => r.CustomerName).IsRequired().HasMaxLength(100);
                eb.Property(r => r.StartDate).IsRequired();
                eb.Property(r => r.EndDate).IsRequired();

                eb.HasOne(r => r.ReservedBy)
                  .WithMany(u => u.Reservations)
                  .HasForeignKey(r => r.ReservedById);
            });
        }
    }
}