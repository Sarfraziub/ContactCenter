using ContactCenter.Data;

namespace EDRSM.API.Interfaces
{
    public interface ICouncillorsRepository
    {
        Task<IReadOnlyList<Councillor>> GetAllCouncillorsAsync();
        Task<Councillor> GetCouncillorByIdAsync(Guid id);
        Task<Councillor> AddCouncillorAsync(Councillor councillor);
        Task<bool> UpdateCouncillorAsync(Councillor councillor);
        Task<bool> DeleteCouncillorAsync(Guid id);
    }
}
