using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cottage>(eb =>
            {
                eb.Property(c => c.Name).IsRequired().HasMaxLength(50);

                // Kluczowe: Description jako wymagane
                eb.Property(c => c.Description).IsRequired();

                eb.OwnsOne(c => c.ContactDetails, cd =>
                {
                    cd.Property(p => p.Price).HasColumnType("decimal(18,2)");
                    // Opcjonalne opisy wewnątrz detali
                    cd.Property(p => p.Description).IsRequired(false);
                });

                eb.HasOne(c => c.Owner)
                  .WithMany(u => u.OwnedCottages)
                  .HasForeignKey(c => c.OwnerId);
            });
        }
    }
}