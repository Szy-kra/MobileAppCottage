using Microsoft.AspNetCore.Identity;

namespace MobileAppCottage.Domain.Entities
{
    // Rozszerzamy standardowego użytkownika o Twoje specyficzne pola
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // --- DLA BUSINESS GUEST ---
        // Jeśli to konto firmowe, wypełniamy te pola
        public string? CompanyName { get; set; }
        public string? TaxId { get; set; } // To jest Twój NIP

        // --- RELACJE (Navigation Properties) ---

        // Jako OWNER: Lista domków, które ten użytkownik posiada
        public virtual ICollection<Cottage> OwnedCottages { get; set; } = new List<Cottage>();

        // Jako GUEST / BUSINESS GUEST: Lista rezerwacji, których ten użytkownik dokonał
        // Nazwa 'Reservations' pasuje do pola 'ReservedBy' w klasie CottageReservation
        public virtual ICollection<CottageReservation> Reservations { get; set; } = new List<CottageReservation>();
    }
}