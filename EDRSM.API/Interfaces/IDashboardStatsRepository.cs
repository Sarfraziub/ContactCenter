using ContactCenter.Data;
using ContactCenter.Data.Identity;
using System.Threading.Tasks;

namespace EDRSM.API.Interfaces
{
    public interface IDashboardStatsRepository
    {
        Task<IReadOnlyList<Incident>> GetAllIncidentAsync(int? incidentTypeId, string? userId);
        Task<IReadOnlyList<EdrsmAppUser>> GetAllEdrsmUsersAsync();
        Task<IReadOnlyList<Faq>> GetAllFaqsAsync();
        Task<IReadOnlyList<Councillor>> GetAllCouncillorsAsync();
        Task<IReadOnlyList<RequestedPaymentPlan>> GetAllPaymentPlansAsync(string? userId);
    }
}
