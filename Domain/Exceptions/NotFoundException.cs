namespace MobileAppCottage.Domain.Exceptions
{
    // Ta klasa reprezentuje sytuację, gdy szukanego elementu nie ma w bazie
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}