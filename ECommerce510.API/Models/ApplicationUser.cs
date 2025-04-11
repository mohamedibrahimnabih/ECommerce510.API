using Microsoft.AspNetCore.Identity;

namespace ECommerce510.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
    }
}
