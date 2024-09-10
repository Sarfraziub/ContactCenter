using ContactCenter.Data;
using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;
using EDRSM.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EDRSM.API.Controllers
{
    public class DashboardStatsController : BaseApiController
    {
        private readonly UserManager<EdrsmAppUser> _userManager;
        private readonly IDashboardStatsRepository _dashboardStatsRepository;

        public DashboardStatsController(
            UserManager<EdrsmAppUser> userManager,
            IDashboardStatsRepository dashboardStatsRepository
            )
        {
            _userManager = userManager;
            _dashboardStatsRepository = dashboardStatsRepository;
        }


        [HttpGet("users")]
        public async Task<ActionResult<IReadOnlyList<EdrsmUserToReturnDto>>> GetAllUsers()
        {
            var users = await _dashboardStatsRepository.GetAllEdrsmUsersAsync();

            var usersToReturn = users.Select(user => new EdrsmUserToReturnDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                IdentificationTypeId = user.IdentificationTypeId,
                IdentificationNumber = user.IdentificationNumber,
                PreferredContactMethodId = user.PreferredContactMethodId,
                CellphoneNumber = user.CellphoneNumber,
                Ward = user.Ward,
                CountryOfOriginId = user.CountryOfOriginId,
                IsAdmin = user.IsAdmin,
                ProfileImageUrl = user.ProfileImageUrl,
                MunicipalityAccountNumber = user.MunicipalityAccountNumber
            }).ToList();

            return Ok(usersToReturn);
        }

        [HttpGet("councillors")]
        public async Task<ActionResult<IReadOnlyList<Councillor>>> GetAllCouncillors()
        {
            var councillors = await _dashboardStatsRepository.GetAllCouncillorsAsync();
            return Ok(councillors);
        }

        [HttpGet("faqs")]
        public async Task<ActionResult<IReadOnlyList<Faq>>> GetAllFaqs()
        {
            var faqs = await _dashboardStatsRepository.GetAllFaqsAsync();
            return Ok(faqs);
        }

        [HttpGet("incidents")]
        public async Task<ActionResult<IReadOnlyList<Incident>>> GetAllIncidents(int? incidentTypeId, string? userId)
        {
            var incidents = await _dashboardStatsRepository.GetAllIncidentAsync(incidentTypeId, userId);
            return Ok(incidents);
        }

        [HttpGet("paymentPlans")]
        public async Task<ActionResult<IReadOnlyList<RequestedPaymentPlan>>> GetAllPaymentPlans(string? userId)
        {
            var paymentPlans = await _dashboardStatsRepository.GetAllPaymentPlansAsync(userId);
            return Ok(paymentPlans);
        }

    }
}
