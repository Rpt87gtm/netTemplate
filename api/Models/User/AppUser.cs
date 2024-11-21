using Microsoft.AspNetCore.Identity;

namespace api.Models.User
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
