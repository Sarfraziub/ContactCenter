using ContactCenter.Data;

namespace EDRSM.API.Interfaces
{
    public interface IFaqRepository
    {
        Task<IReadOnlyList<Faq>> GetAllAsync();
        Task<Faq> GetByIdAsync(Guid id);
        Task<Faq> AddAsync(Faq faq);
        Task<bool> UpdateAsync(Faq faq);
        Task<bool> DeleteAsync(Guid id);
        Task<IReadOnlyList<string>> GetUniqueCategoriesAsync();
    }
}
