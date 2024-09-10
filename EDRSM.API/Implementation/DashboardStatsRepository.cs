using ContactCenter.Data.Identity;
using ContactCenter.Data;
using Microsoft.EntityFrameworkCore;
using EDRSM.API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EDRSM.API.Implementation
{
    public class DashboardStatsRepository : IDashboardStatsRepository
    {
        private readonly EdrsmIdentityDbContext _identityContext;
        private readonly EDRSMContext _edrsmContext;
        private readonly UserManager<EdrsmAppUser> _userManager;

        public DashboardStatsRepository(
            EdrsmIdentityDbContext identityContext, 
            EDRSMContext edrsmContext, 
            UserManager<EdrsmAppUser> userManager
            )
        {
            _identityContext = identityContext;
            _edrsmContext = edrsmContext;
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<Councillor>> GetAllCouncillorsAsync()
        {
            return await _edrsmContext.Councillors.ToListAsync();
        }

        public async Task<IReadOnlyList<EdrsmAppUser>> GetAllEdrsmUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IReadOnlyList<Faq>> GetAllFaqsAsync()
        {
            return await _edrsmContext.Faqs.ToListAsync();
        }

        public async Task<IReadOnlyList<Incident>> GetAllIncidentAsync(int? incidentTypeId, string? userId)
        {
            var query = _edrsmContext.Incidents
                .Include(i => i.IncidentAudits)
                .AsQueryable();

            if (incidentTypeId.HasValue)
            {
                query = query.Where(i => i.IncidentTypeId == incidentTypeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(i => i.UserId == userId);
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<RequestedPaymentPlan>> GetAllPaymentPlansAsync(string? userId)
        {
            var query = _edrsmContext.RequestedPaymentPlans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(i => i.UserId == userId);
            }

            return await query.ToListAsync();
        }
    
    }
}
