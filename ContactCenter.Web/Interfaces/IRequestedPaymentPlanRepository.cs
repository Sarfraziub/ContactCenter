using ContactCenter.Data;

namespace EDRSM.API.Interfaces
{
    public interface IRequestedPaymentPlanRepository
    {
        Task<IReadOnlyList<RequestedPaymentPlan>> GetAllRequestedPaymentPlansAsync();
        Task<RequestedPaymentPlan> GetRequestedPaymentPlanByIdAsync(Guid id);
        Task<RequestedPaymentPlan> CreateRequestedPaymentPlanAsync(RequestedPaymentPlan plan);
        Task<bool> UpdateRequestedPaymentPlanAsync(RequestedPaymentPlan plan);
        Task<bool> DeleteRequestedPaymentPlanAsync(Guid id);
    }
}
