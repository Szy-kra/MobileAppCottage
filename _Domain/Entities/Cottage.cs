using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileAppCottage.Domain.Entities
{
    public class Cottage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // Zostawiamy TYLKO to jedno Name

        [Required(ErrorMessage = "Opis jest wymagany.")]
        public string Description { get; set; } = string.Empty;

        public string? About { get; set; }

        public CottageDetails ContactDetails { get; set; } = new CottageDetails();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? EncodedName { get; set; }

        public string? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }

        public virtual List<CottageImage> Images { get; set; } = new List<CottageImage>();

        public virtual List<CottageReservation> Reservations { get; set; } = new List<CottageReservation>();

        public void EncodeName()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                EncodedName = Name.ToLower().Replace(" ", "-");
            }
        }
    }
}