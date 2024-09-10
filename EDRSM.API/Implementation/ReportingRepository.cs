using ContactCenter.Data;
using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;
using EDRSM.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EDRSM.API.Implementation
{
    public class ReportingRepository : IReportingRepository
    {

        private readonly EDRSMContext _edrsmContext;

        public ReportingRepository(
            EDRSMContext edrsmContext
            )
        {
            _edrsmContext = edrsmContext;
        }

        public async Task<IReadOnlyList<IncidentHeading>> GetIncidentHeadingsAsync()
        {
            return await _edrsmContext.IncidentHeadings.ToListAsync();
        }

        public async Task<IReadOnlyList<IncidentStatus>> GetIncidentStatusesAsync()
        {
            return await _edrsmContext.IncidentStatuses.ToListAsync();
        }

        public async Task<IReadOnlyList<IncidentType>> GetIncidentTypesAsync()
        {
            return await _edrsmContext.IncidentTypes.ToListAsync();
        }

        public async Task<Incident> SubmitIncidentReportAsync(SubmitIncidentDto submitIncidentDto, string incidentReference)
        {
            var incident = new Incident
            {
                StatusId = submitIncidentDto.StatusId,
                Reference = incidentReference,
                TimePosted = DateTime.Now,
                LocationAddress = submitIncidentDto.LocationAddress,
                LocationCoordinates = submitIncidentDto.LocationCoordinates,
                ContactNumber = submitIncidentDto.ContactNumber,
                UserId = submitIncidentDto.AnonymousSubmission == true ? "Anonymous Submission" : submitIncidentDto.UserId,
                IncidentHeadingId = submitIncidentDto.IncidentHeadingId,
                HeadingName = submitIncidentDto.HeadingName,
                IncidentTypeId = submitIncidentDto.IncidentTypeId,
                TypeName = submitIncidentDto.TypeName,

            };

            await _edrsmContext.Incidents.AddAsync(incident);
            await _edrsmContext.SaveChangesAsync();

            var incidentAudit = new IncidentAudit
            {
                IncidentId = incident.Id,
                StatusId = submitIncidentDto.StatusId,
                StatusName = submitIncidentDto.StatusName,
                StatusChangeTime = incident.TimePosted,
                UserId = submitIncidentDto.AnonymousSubmission == true ? "Anonymous Submission" : submitIncidentDto.UserId,
                ShortSummary = submitIncidentDto.ShortSummary,
                DetailedDescription = submitIncidentDto.DetailedDescription,
            };

            await _edrsmContext.IncidentAudits.AddAsync(incidentAudit);
            await _edrsmContext.SaveChangesAsync();

            return incident;
        }


        public async Task<IncidentAudit> SubmitIncidentAuditAsync(SubmitIncidentAuditDto submitIncidentAuditDto)
        {
            var incidentAudit = new IncidentAudit
            {
                IncidentId = submitIncidentAuditDto.IncidentId,
                StatusId = submitIncidentAuditDto.StatusId,
                StatusName = submitIncidentAuditDto.StatusName,
                StatusChangeTime = DateTime.Now,
                UserId = submitIncidentAuditDto.UserId,
                ShortSummary = submitIncidentAuditDto.ShortSummary,
                DetailedDescription = submitIncidentAuditDto.DetailedDescription,
            };

            await _edrsmContext.IncidentAudits.AddAsync(incidentAudit);
            await _edrsmContext.SaveChangesAsync();

            return incidentAudit;
        }
    }
}
