using ContactCenter.Data;
using ContactCenter.Data.Entities;
using ContactCenter.Lib;
using ContactCenter.Web.API;
using EDRSM.API.DTOs;
using EDRSM.API.Errors;
using EDRSM.API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDRSM.API.Controllers
{
    public class ReportingController : SysAPIController
    {
        private readonly UserManager<ContactUser> _userManager;

        public ReportingController(
            UserManager<ContactUser> userManager
            )
        {
            _userManager = userManager;
        }

        [HttpGet("incident-types")]
        public async Task<ActionResult<IReadOnlyList<ContactCenter.Data.TicketType>>> GeIncidentTypes()
        {
            return Ok(await Db.TicketTypes.ToListAsync());
        }

        [HttpGet("incident-headings")]
        public async Task<ActionResult<IReadOnlyList<TicketHeading>>> GeIncidentHeadings()
        {
            return Ok(await Db.TicketHeadings.ToListAsync());
        }

        [HttpGet("incident-statuses")]
        public async Task<ActionResult<IReadOnlyList<ContactCenter.Data.TicketStatus>>> GetIncidentStatuses()
        {
            return Ok(await Db.TicketStatuses.ToListAsync());
        }
        [HttpPost("submit-incident")]
        public async Task<ActionResult<ReturnTicketDto>> SubmitIncidentReport([FromBody] SubmitTicketDto submitIncidentDto)
        {
            // Find the user who submitted the incident
            var user = await _userManager.FindByIdAsync(submitIncidentDto.CreatorId);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            // Generate incident reference
            var incidentReference = IncidentReferenceGenerator.GenerateIncidentReference();

            // Get all active agents
            var agents = await Db.Users
               .Where(u => u.RoleId == (int)UserRole.AGENT && u.IsActive == true)
               .ToListAsync();

            // Get today's date
            var today = DateTime.Today.Date;

            // Get the list of agents who have already been assigned a ticket today
            var assignedAgentsToday = await Db.Tickets
                .Where(t => t.CreationDate.Date == today)
                .Select(t => t.AssigneeId)
                .ToListAsync();

            // Find available agents who have not been assigned tickets today
            var availableAgents = agents.Where(a => !assignedAgentsToday.Contains(a.Id)).ToList();

            // Determine the assignee (agent) for the ticket
            Guid adminId = Guid.NewGuid();
            if (availableAgents.Any())
                adminId = availableAgents.First().Id;
            else if (agents.Count > 0)
                adminId = agents.FirstOrDefault().Id;
            else
                adminId = Db.Users.FirstOrDefault().Id;

            // Create the new ticket
            var ticket = new Ticket
            {
                StatusId = submitIncidentDto.StatusId,
                CreationDate = DateTime.Now,
                LocationAddress = submitIncidentDto.LocationAddress,
                //CreatorId = submitIncidentDto.CreatorId,
                Description = submitIncidentDto.Description,
                TypeId = submitIncidentDto.TypeId,
                AssigneeId = adminId,
                //LocationId = submitIncidentDto.LocationId,
                //CategoryId = submitIncidentDto.CategoryId,
                AssignmentDate = DateTime.Now,
                NotesJson = submitIncidentDto.NotesJson,
                Number = incidentReference,
                TicketHeadingId = submitIncidentDto.TicketHeadingId
            };

            // Add the ticket to the database and save changes
            await Db.Tickets.AddAsync(ticket);
            try
            {
                await Db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            // Create and add an audit log for the ticket
            var incidentAudit = new TicketAudit
            {
                TicketId = ticket.Id,
                StatusId = submitIncidentDto.StatusId,
                StatusName = Db.TicketStatuses.FirstOrDefault(x => x.Id == submitIncidentDto.StatusId)?.StatusName,
                StatusChangeTime = ticket.AssignmentDate ?? DateTime.Now,
                UserId = submitIncidentDto.UserId,
                // ShortSummary = ticketDto.Description,
                Description = submitIncidentDto.Description
            };

            await Db.TicketAudites.AddAsync(incidentAudit);
            await Db.SaveChangesAsync();

            // Prepare the return DTO
            var returnIncidentDto = new ReturnTicketDto
            {
                Id = ticket.Id,
                Reference = incidentReference,
                StatusId = ticket.StatusId,
            };

            // Return the created ticket details
            return Ok(returnIncidentDto);
        }

        [HttpPost("submit-incident-audit")]
        public async Task<ActionResult<TicketAudit>> SubmitIncidentAudit([FromBody] SubmitIncidentAuditDto submitIncidentAuditDto)
        {
            var user = await _userManager.FindByIdAsync(submitIncidentAuditDto.UserId);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            // Create the new incident audit
            var incidentAudit = new TicketAudit
            {
                TicketId = submitIncidentAuditDto.TicketId,
                StatusId = submitIncidentAuditDto.StatusId,
                StatusName = submitIncidentAuditDto.StatusName,
                StatusChangeTime = DateTime.Now,
                UserId = submitIncidentAuditDto.UserId,
                Description = submitIncidentAuditDto.Description,
                //ShortSummary = submitIncidentAuditDto.ShortSummary,
                NameOfUpdater = submitIncidentAuditDto.UpdatedBy
            };

            await Db.TicketAudites.AddAsync(incidentAudit);

            try
            {
                await Db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, "Error saving incident audit"));
            }

            return Ok(incidentAudit);
        }


    }
}
