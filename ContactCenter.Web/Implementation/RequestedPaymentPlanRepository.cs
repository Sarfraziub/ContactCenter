using ContactCenter.Data;
using EDRSM.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EDRSM.API.Implementation
{
    public class RequestedPaymentPlanRepository : IRequestedPaymentPlanRepository
    {
        private readonly EDRSMContext _edrsmContext;

        public RequestedPaymentPlanRepository(EDRSMContext edrsmContext)
        {
            _edrsmContext = edrsmContext;
        }

        public async Task<IReadOnlyList<RequestedPaymentPlan>> GetAllRequestedPaymentPlansAsync()
        {
            return await _edrsmContext.RequestedPaymentPlans.ToListAsync();
        }

        public async Task<RequestedPaymentPlan> GetRequestedPaymentPlanByIdAsync(Guid id)
        {
            return await _edrsmContext.RequestedPaymentPlans.FindAsync(id);
        }

        public async Task<RequestedPaymentPlan> CreateRequestedPaymentPlanAsync(RequestedPaymentPlan plan)
        {
            await _edrsmContext.RequestedPaymentPlans.AddAsync(plan);
            await _edrsmContext.SaveChangesAsync();
            return plan;
        }

        public async Task<bool> UpdateRequestedPaymentPlanAsync(RequestedPaymentPlan plan)
        {
            var existingPlan = await _edrsmContext.RequestedPaymentPlans.FindAsync(plan.Id);
            if (existingPlan == null)
            {
                return false;
            }

            existingPlan.ReviewStatus = plan.ReviewStatus;
            existingPlan.ReviewComment = plan.ReviewComment;
            existingPlan.RequestReviewedDate = DateTime.Now;
            existingPlan.AdminReviewerId = plan.AdminReviewerId;
            existingPlan.AdminReviewerName = plan.AdminReviewerName;
            existingPlan.AdminReviewerSurname = plan.AdminReviewerSurname;

            await _edrsmContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRequestedPaymentPlanAsync(Guid id)
        {
            var plan = await _edrsmContext.RequestedPaymentPlans.FindAsync(id);
            if (plan == null)
            {
                return false;
            }

            _edrsmContext.RequestedPaymentPlans.Remove(plan);
            await _edrsmContext.SaveChangesAsync();
            return true;
        }
    }
}
