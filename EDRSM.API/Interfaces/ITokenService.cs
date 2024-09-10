using ContactCenter.Data.Identity;

namespace EDRSM.API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(EdrsmAppUser user);
    }
}
