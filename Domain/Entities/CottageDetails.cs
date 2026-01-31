namespace MobileAppCottage.Domain.Entities
{
    public class CottageDetails  // Owned Types
    {
        public string? Description { get; set; }

        // Zmieniamy na nullable, żeby cena nie musiała być podana od razu
        public decimal? Price { get; set; }

        // Tutaj Twoja prośba: MaxPersons staje się opcjonalne
        public int? MaxPersons { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
    }
}