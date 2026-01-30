using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileAppCottage.Domain.Entities
{
    public class CottageReservation
    {
        [Key]
        public int Id { get; set; }

        // Powiązanie z domkiem
        [Required]
        public int CottageId { get; set; }

        [ForeignKey("CottageId")]
        public virtual Cottage? Cottage { get; set; }

        // Daty rezerwacji
        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Data zakończenia jest wymagana.")]
        public DateTime EndDate { get; set; }

        // Dane klienta (Osoba, która fizycznie przyjedzie)
        [Required(ErrorMessage = "Nazwa klienta nie może być pusta.")]
        public string CustomerName { get; set; } = string.Empty;

        // ZMIANA: Telefon zamiast maila (do WhatsApp)
        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        public string CustomerPhone { get; set; } = string.Empty;

        // Status płatności - Owner zmienia to na true po zaksięgowaniu przelewu
        public bool IsPaid { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // --- DOPISANE DLA IDENTITY I BUSINESS GUEST ---

        // 1. ID użytkownika, który kliknął "Rezerwuj" (Guest lub BusinessGuest)
        public string? ReservedById { get; set; }

        // 2. Właściwość nawigacyjna do Usera (Identity)
        [ForeignKey("ReservedById")]
        public virtual User? ReservedBy { get; set; }

        // 3. Pole dla BusinessGuest: czy rezerwuje dla siebie, czy dla pracownika?
        // Jeśli true, dane w CustomerName będą danymi pracownika, a ReservedBy to będzie konto firmowe.
        public bool IsForSomeoneElse { get; set; } = false;

        // ----------------------------------------------
    }
}