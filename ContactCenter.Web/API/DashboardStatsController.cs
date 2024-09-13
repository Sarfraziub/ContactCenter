using ContactCenter.Data;
using ContactCenter.Data.Entities;
using ContactCenter.Web.API;
using EDRSM.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Security.Claims;

namespace EDRSM.API.Controllers
{
    public class DashboardStatsController : SysAPIController
    {
        private readonly UserManager<ContactUser> _userManager;
        public DashboardStatsController(
            UserManager<ContactUser> userManager
            )
        {
            _userManager = userManager;
        }


        [HttpGet("users")]
        public async Task<ActionResult<IReadOnlyList<EdrsmUserToReturnDto>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

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
               CellphoneNumber = user.PhoneNumber.ToString(),
                Ward = user.Ward.ToString(),
                CountryOfOriginId = user.CountryOfOriginId,
                IsAdmin = user.IsAdmin,
               // ProfileImageUrl = user.ProfileImageUrl,
                MunicipalityAccountNumber = user.MunicipalityAccountNumber
            }).ToList();

            return Ok(usersToReturn);
        }

        [HttpGet("councillors")]
        public async Task<ActionResult<IReadOnlyList<Councillor>>> GetAllCouncillors()
        {
            var councillors = await Db.Councillors.ToListAsync();
            return Ok(councillors);
        }

        [HttpGet("faqs")]
        public async Task<ActionResult<IReadOnlyList<Faq>>> GetAllFaqs()
        {
            var faqs = await Db.Faqs.ToListAsync();
            return Ok(faqs);
        }

        [HttpGet("incidents")]
        public async Task<ActionResult<IReadOnlyList<Ticket>>> GetAllIncidents(int? incidentTypeId, string? userId)
        {
            var incidents = await GetAllIncidentAsync(incidentTypeId, userId);
            return Ok(incidents);
        }

        [HttpGet("paymentPlans")]
        public async Task<ActionResult<IReadOnlyList<RequestedPaymentPlan>>> GetAllPaymentPlans(string? userId)
        {
            var query = Db.RequestedPaymentPlans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(i => i.UserId == userId);
            }

            var paymentPlans = await query.ToListAsync();
            return Ok(paymentPlans);
        }

        [HttpGet("tickets")]
        public async Task<ActionResult<IReadOnlyList<Ticket>>> GetAllTickets()
        {
            var tickets = await Db.Tickets.Include(x => x.TicketAudits).ToListAsync();
            return Ok(tickets);
        }
        private async Task<IReadOnlyList<Ticket>> GetAllIncidentAsync(int? incidentTypeId, string? userId)
        {
            var query = Db.Tickets
                .Include(i => i.TicketAudits)
                .AsQueryable();

            if (incidentTypeId.HasValue)
            {
                query = query.Where(i => i.TypeId == incidentTypeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                //query = query.Where(i => i.AssigneeId == userId);
            }

            return await query.ToListAsync();
        }

    }
}
