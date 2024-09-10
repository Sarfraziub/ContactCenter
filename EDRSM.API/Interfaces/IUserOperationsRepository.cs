using ContactCenter.Data;
using ContactCenter.Data.Identity;

namespace EDRSM.API.Interfaces
{
    public interface IUserOperationsRepository
    {
        Task<IReadOnlyList<Country>> GetCountryDetailsAsync();
        Task<IReadOnlyList<IdentificationType>> GetIdentificationTypesAsync();
        Task<IReadOnlyList<PreferredContactMethod>> GetContactMethodsAsync();
        Task UpdateUserProfile(EdrsmAppUser user);
    }
}
