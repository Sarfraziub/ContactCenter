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
        private readonly CCDbContext _edrsmContext;
        private readonly UserManager<ContactUser> _userManager;

        public DashboardStatsRepository(
            EdrsmIdentityDbContext identityContext, 
            CCDbContext edrsmContext, 
            UserManager<ContactUser> userManager
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

        public async Task<IReadOnlyList<ContactUser>> GetAllEdrsmUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IReadOnlyList<Faq>> GetAllFaqsAsync()
        {
            return await _edrsmContext.Faqs.ToListAsync();
        }

        public async Task<IReadOnlyList<Ticket>> GetAllIncidentAsync(int? incidentTypeId, string? userId)
        {
            //    var query = _edrsmContext.Incidents
            //        .Include(i => i.IncidentAudits)
            //        .AsQueryable();

            //    if (incidentTypeId.HasValue)
            //    {
            //        query = query.Where(i => i.IncidentTypeId == incidentTypeId.Value);
            //    }

            //    if (!string.IsNullOrWhiteSpace(userId))
            //    {
            //        query = query.Where(i => i.UserId == userId);
            //    }

            //    return await query.ToListAsync();
            return new List<Ticket>();
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
