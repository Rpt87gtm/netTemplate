using api.Models.User;

namespace api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
