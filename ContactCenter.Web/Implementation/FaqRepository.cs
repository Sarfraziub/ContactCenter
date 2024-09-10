using ContactCenter.Data;
using EDRSM.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EDRSM.API.Implementation
{
    public class FaqRepository : IFaqRepository
    {
        private readonly EDRSMContext _edrsmContext;

        public FaqRepository(
            EDRSMContext edrsmContext
            )
        {
            _edrsmContext = edrsmContext;
        }

        public async Task<IReadOnlyList<Faq>> GetAllAsync()
        {
            return await _edrsmContext.Faqs.ToListAsync();
        }

        public async Task<Faq> GetByIdAsync(Guid id)
        {
            return await _edrsmContext.Faqs.FindAsync(id);
        }

        public async Task<Faq> AddAsync(Faq faq)
        {
            await _edrsmContext.Faqs.AddAsync(faq);
            await _edrsmContext.SaveChangesAsync();
            return faq;
        }

        public async Task<bool> UpdateAsync(Faq faq)
        {
            var existingFaq = await _edrsmContext.Faqs.FindAsync(faq.Id);
            if (existingFaq == null)
            {
                return false;
            }

            existingFaq.Category = faq.Category;
            existingFaq.ByCategorySorter = faq.ByCategorySorter;
            existingFaq.Question = faq.Question;
            existingFaq.Answer = faq.Answer;

            await _edrsmContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var faq = await _edrsmContext.Faqs.FindAsync(id);
            if (faq == null)
            {
                return false;
            }

            _edrsmContext.Faqs.Remove(faq);
            await _edrsmContext.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<string>> GetUniqueCategoriesAsync()
        {
            return await _edrsmContext.Faqs
                .Select(f => f.Category)
                .Distinct()
                .ToListAsync();
        }
    }
}
