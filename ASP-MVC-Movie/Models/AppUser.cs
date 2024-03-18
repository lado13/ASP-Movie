using Microsoft.AspNetCore.Identity;

namespace ASP_MVC_Movie.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }  
    }
}
