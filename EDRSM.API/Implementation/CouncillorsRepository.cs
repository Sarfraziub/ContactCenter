using ContactCenter.Data;
using EDRSM.API.DTOs;
using EDRSM.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EDRSM.API.Implementation
{
    public class CouncillorsRepository : ICouncillorsRepository
    {
        private readonly EDRSMContext _edrsmContext;
        private readonly IPhotoService _photoService;

        public CouncillorsRepository(
            EDRSMContext edrsmContext,
            IPhotoService photoService
            )
        {
            _edrsmContext = edrsmContext;
            _photoService = photoService;
        }

        public async Task<IReadOnlyList<Councillor>> GetAllCouncillorsAsync()
        {
            return await _edrsmContext.Councillors.ToListAsync();
        }

        public async Task<Councillor> GetCouncillorByIdAsync(Guid id)
        {
            return await _edrsmContext.Councillors.FindAsync(id);
        }

        public async Task<Councillor> AddCouncillorAsync(Councillor councillor)
        {
            _edrsmContext.Councillors.Add(councillor);
            await _edrsmContext.SaveChangesAsync();
            return councillor;
        }

        public async Task<bool> UpdateCouncillorAsync(Councillor councillor)
        {
            var existingCouncillor = await _edrsmContext.Councillors.FindAsync(councillor.Id);
            if (existingCouncillor == null)
            {
                return false;
            }

            existingCouncillor.Name = councillor.Name;
            existingCouncillor.ContactNumber = councillor.ContactNumber;
            existingCouncillor.Ward = councillor.Ward;
            existingCouncillor.Image = councillor.Image;
            existingCouncillor.CloudinaryPublicId = councillor.CloudinaryPublicId;

            await _edrsmContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCouncillorAsync(Guid id)
        {
            var councillor = await _edrsmContext.Councillors.FindAsync(id);
            if (councillor == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(councillor.CloudinaryPublicId))
            {
                await _photoService.DeletePhotoAsync(councillor.CloudinaryPublicId);
            }

            _edrsmContext.Councillors.Remove(councillor);
            await _edrsmContext.SaveChangesAsync();
            return true;
        }
    }
}
