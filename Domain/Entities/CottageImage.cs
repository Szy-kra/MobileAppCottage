namespace MobileAppCottage.Domain.Entities
{
    // Relacja jeden do wielu - jeden domek może mieć wiele zdjęć

    public class CottageImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = default!;

        // Klucz obcy - to pole mówi bazie, do którego domku należy to zdjęcie
        public int CottageId { get; set; }

        // Właściwość nawigacyjna
        // '?' pozwala na nullowalność, co zapobiega błędom walidacji podczas zapisu samego zdjęcia
        public Cottage? Cottage { get; set; }
    }

}