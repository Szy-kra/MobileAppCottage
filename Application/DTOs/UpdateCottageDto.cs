using System.ComponentModel.DataAnnotations;

namespace MobileAppCottage.Application.DTOs
{
    public class UpdateCottageDto
    {
        [Required(ErrorMessage = "Nazwa domku jest wymagana.")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? MaxPersons { get; set; }

        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}