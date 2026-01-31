using Microsoft.AspNetCore.Identity;

namespace MobileAppCottage.Domain.Entities
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}