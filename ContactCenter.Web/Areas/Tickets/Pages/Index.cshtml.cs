using ContactCenter.Data;
using ContactCenter.Data.Entities;
using ContactCenter.Lib;
using ContactCenter.Web.DTOs;
using ContactCenter.Web.Pages;
using EDRSM.API.DTOs;
using EDRSM.API.Helpers;
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
        private readonly CCDbContext _context; // Database context

        public List<ContactUser> ContactUser { get; set; }

        public IndexModel(CCDbContext context)
        {
            _context = context;
        }

        public bool FlgCurrentUserOnline { get; set; }
        public User Assignee { get; set; }
        public List<TicketStatus> Statuses { get; set; }

        public void OnGet(Guid? id, int? p, int? ps, string q)
        {
            PageTitle = Title = "Tickets";
            this.Statuses = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().ToList();

            var dbtickets = _context.Tickets
                .Include(c => c.TicketAudits)
                .Include(c => c.Assignee)
                .OrderByDescending(x => x.Id)
                .ToList();

            var incidentList = dbtickets.Select(incident =>
            {
                var statusName = Enum.GetName(typeof(TicketStatus), incident.StatusId);
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
                    StatusId = (int)model.StatusId,
                    StatusName = model.StatusId.ToString(),
                    Description = model.Description,
                    UpdatedBy = user.LoginId
                };
                var incidentAudit = await submitIncidentAudit(incidentAuditDto);
                incident.StatusId = model.StatusId;
                _context.SaveChanges();
            }
            return new JsonResult("my result");
        }

        private async Task<TicketAudit> submitIncidentAudit(SubmitIncidentAuditDto submitIncidentAuditDto)
        {
            var incidentAudit = new TicketAudit
            {
                TicketId = submitIncidentAuditDto.TicketId,
                StatusId = (TicketStatus)submitIncidentAuditDto.StatusId, // Convert int back to enum
                StatusName = submitIncidentAuditDto.StatusName,
                StatusChangeTime = DateTime.Now,
                UserId = submitIncidentAuditDto.UserId,
                Description = submitIncidentAuditDto.Description,
                NameOfUpdater = submitIncidentAuditDto.UpdatedBy
            };

            await Db.TicketAudites.AddAsync(incidentAudit);
            await Db.SaveChangesAsync();
            return incidentAudit;
        }
    }
}