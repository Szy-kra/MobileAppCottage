using Microsoft.AspNetCore.Identity;

namespace MobileAppCottage._Domain.Entities
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}