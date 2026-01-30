namespace MobileAppCottage.Application.DTOs
{
    public class ReservationDto
    {
        // Numer ID rezerwacji (zlecenia)
        public int Id { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public bool IsAvailable { get; set; } = false;
    }
}