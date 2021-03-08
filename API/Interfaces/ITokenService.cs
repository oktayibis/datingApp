using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        // intreface signature olarak bilinir.

        string CreateToken(AppUser user);
    }
}