using ContactCenter.Data;
using ContactCenter.Data.Identity;
using ContactCenter.Lib;

//using ContactCenter.Lib;
using ContactCenter.Web.DTOs;
using ContactCenter.Web.Pages;
using EDRSM.API.DTOs;
using EDRSM.API.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList;

namespace ContactCenter.Web.Areas.Tickets.Pages
{
    public class IndexModel : SysListPageModel<Ticket>
    {
        private readonly IReportingRepository _reportingRepo;
        private readonly EDRSMContext _context; // Database context

        public List<ContactUser> ContactUser { get; set; }

        public IndexModel(IReportingRepository reportingRepo, EDRSMContext context)
        {
            _reportingRepo = reportingRepo;
            _context = context;
        }

        public bool FlgCurrentUserOnline { get; set; }
        public User Assignee { get; set; }
        public List<Data.TicketStatus> Statuses { get; set; }

        public void OnGet(Guid? id, int? p, int? ps, string q)
        {
            PageTitle = Title = "Tickets";
            this.Statuses = _context.TicketStatuses.ToList();

            var dbtickets = _context.Tickets
                .Include(c => c.TicketStatus)
                .Include(c => c.TicketAudits)
                .Include(c => c.Assignee)
                .OrderByDescending(x => x.Id)
                .ToList();

            var incidentList = dbtickets.Select(incident =>
            {
                var statusName = this.Statuses
                    .Where(user => user.Id == incident.StatusId)
                    .Select(user => user.StatusName)
                    .FirstOrDefault();

                incident.StatusName = statusName;
                return incident;
            }).AsQueryable();

            // Fetch ContactUser data
            ContactUser = _context.ContactUsers.ToList();

            // If the user is an agent, only show their assigned tickets
            User currentUserIsAgent = _context.Users
                .Where(x => x.Id == CurrentUserId)
                .Where(u => u.RoleId == (int)UserRole.AGENT)
                .FirstOrDefault();

            if (currentUserIsAgent != null)
            {
                incidentList = incidentList.Where(x => x.CreatorId == CurrentUserId.ToString()).AsQueryable();
            }

            List = new PagedList<Ticket>(incidentList, p ?? 1, ps ?? DefaultPageSize);
            PageSubTitle = "ticket".ToQuantity(List.TotalItemCount) + " found..";
        }

        [HttpPost]
        public async Task<IActionResult> OnPostStatus([FromBody] StatusDto model)
        {
            Ticket incident = _context.Tickets.Where(x => x.Id == model.TicketId).FirstOrDefault();
            if (incident != null)
            {
                var user = _context.Users.Where(x => x.Id == CurrentUserId).FirstOrDefault();
                SubmitIncidentAuditDto incidentAuditDto = new SubmitIncidentAuditDto()
                {
                    TicketId = model.TicketId,
                    StatusId = model.StatusId,
                    StatusName = model.StatusName,
                    //UserId = incident.UserId,
                    // ShortSummary = model.Summary,
                    Description = model.Description,
                    UpdatedBy = user.LoginId

                };
                var incidentAudit = await _reportingRepo.SubmitIncidentAuditAsync(incidentAuditDto);
                incident.StatusId = model.StatusId;
                _context.SaveChanges();
            }
            return new JsonResult("my result");
        }
    }
}