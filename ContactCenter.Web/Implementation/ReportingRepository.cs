using ContactCenter.Data;
using ContactCenter.Data.Identity;
using ContactCenter.Lib;
using EDRSM.API.DTOs;
using EDRSM.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using TicketStatus = ContactCenter.Data.TicketStatus;
using TicketType = ContactCenter.Data.TicketType;

namespace EDRSM.API.Implementation
{
    public class ReportingRepository : IReportingRepository
    {

        private readonly CCDbContext _edrsmContext;

        public ReportingRepository(
            CCDbContext edrsmContext
            )
        {
            _edrsmContext = edrsmContext;
        }

        public async Task<IReadOnlyList<TicketHeading>> GetIncidentHeadingsAsync()
        {
            return await _edrsmContext.TicketHeadings.ToListAsync();
        }

        public async Task<IReadOnlyList<TicketStatus>> GetIncidentStatusesAsync()
        {
            return await _edrsmContext.TicketStatuses.ToListAsync();
        }

        public async Task<IReadOnlyList<TicketType>> GetIncidentTypesAsync()
        {
            return await _edrsmContext.TicketTypes.ToListAsync();
        }

        public async Task<Ticket> SubmitIncidentReportAsync(SubmitTicketDto ticketDto, string incidentReference)
        {

            var agents = await _edrsmContext.Users
               .Where(u => u.RoleId == (int)UserRole.AGENT && u.IsActive == true)
               .ToListAsync();

            var today = DateTime.Today.Date;

            var assignedAgentsToday = await _edrsmContext.Tickets
                .Where(t => t.CreationDate.Date == today)
                .Select(t => t.AssigneeId)
                .ToListAsync();
            var availableAgents = agents.Where(a => !assignedAgentsToday.Contains(a.Id)).ToList();
            Guid adminId = Guid.NewGuid();
            if (availableAgents.Any())
                adminId = availableAgents.First().Id;
            else
            {
                if (agents.Count > 0)
                    adminId = agents.FirstOrDefault().Id;
                else
                    adminId = _edrsmContext.Users.FirstOrDefault().Id;
            }

            var ticket = new Ticket
            {
                StatusId = ticketDto.StatusId,
                CreationDate = DateTime.Now,
                LocationAddress = ticketDto.LocationAddress,
                //ContactId = submitIncidentDto.ContactId,
                CreatorId = ticketDto.CreatorId, //submitIncidentDto.AnonymousSubmission == true ? "Anonymous Submission" : submitIncidentDto.UserId,
                Description = ticketDto.Description,
                TypeId = ticketDto.TypeId,
                AssigneeId = adminId,
                AssignmentDate = DateTime.Now,
                //LocationId = submitIncidentDto.LocationId,
                //CategoryId = submitIncidentDto.CategoryId,
                NotesJson = ticketDto.NotesJson,
                Number = incidentReference,
                TicketHeadingId = ticketDto.TicketHeadingId
    };

            await _edrsmContext.Tickets.AddAsync(ticket);
            try
            {
                await _edrsmContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            
            
            var incidentAudit = new TicketAudit
            {
                TicketId = ticket.Id,
                StatusId = ticketDto.StatusId,
                StatusName = _edrsmContext.TicketStatuses.Where(x=>x.Id == ticketDto.StatusId).FirstOrDefault().StatusName,
                StatusChangeTime = ticket.AssignmentDate??(DateTime)(ticket.AssignmentDate),
                UserId = ticketDto.UserId,
                // ShortSummary = ticketDto.Description,
                Description = ticketDto.Description,
            };

            await _edrsmContext.TicketAudites.AddAsync(incidentAudit);
            await _edrsmContext.SaveChangesAsync();

            return ticket;
        }


        public async Task<TicketAudit> SubmitIncidentAuditAsync(SubmitIncidentAuditDto submitIncidentAuditDto)
        {
            var incidentAudit = new TicketAudit
            {
                TicketId = submitIncidentAuditDto.TicketId,
                StatusId = submitIncidentAuditDto.StatusId,
                StatusName = submitIncidentAuditDto.StatusName,
                StatusChangeTime = DateTime.Now,
                UserId = submitIncidentAuditDto.UserId,
                //ShortSummary = submitIncidentAuditDto.ShortSummary,
                Description = submitIncidentAuditDto.Description,
                NameOfUpdater = submitIncidentAuditDto.UpdatedBy
            };

            await _edrsmContext.TicketAudites.AddAsync(incidentAudit);
            await _edrsmContext.SaveChangesAsync();

            return incidentAudit;
        }
    }
}
