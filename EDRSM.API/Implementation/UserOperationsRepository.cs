using ContactCenter.Data;
using ContactCenter.Data.Identity;
using EDRSM.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EDRSM.API.Implementation
{
    public class UserOperationsRepository : IUserOperationsRepository
    {
        private readonly EDRSMContext _edrsmContext;
        private readonly EdrsmIdentityDbContext _edrsmIdentityDbContext;

        public UserOperationsRepository(
            EDRSMContext edrsmContext,
            EdrsmIdentityDbContext edrsmIdentityDbContext
            )
        {
            _edrsmContext = edrsmContext;
            _edrsmIdentityDbContext = edrsmIdentityDbContext;
        }

        public async Task<IReadOnlyList<PreferredContactMethod>> GetContactMethodsAsync()
        {
            return await _edrsmContext.PreferredContactMethods.ToListAsync();
        }

        public async Task<IReadOnlyList<Country>> GetCountryDetailsAsync()
        {
            return await _edrsmContext.Countries.ToListAsync();
        }

        public async Task<IReadOnlyList<IdentificationType>> GetIdentificationTypesAsync()
        {
            return await _edrsmContext.IdentificationTypes.ToListAsync();
        }

        public async Task UpdateUserProfile(EdrsmAppUser user)
        {
            _edrsmIdentityDbContext.Entry(user).State = EntityState.Modified;
            await _edrsmIdentityDbContext.SaveChangesAsync();
        }
    }
}
