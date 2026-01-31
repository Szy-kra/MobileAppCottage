namespace MobileAppCottage.Application.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsAvailable { get; set; } = false;
    }
}