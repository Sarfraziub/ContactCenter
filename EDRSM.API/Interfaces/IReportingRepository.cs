using ContactCenter.Data;
using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;

namespace EDRSM.API.Interfaces
{
    public interface IReportingRepository
    {
        Task<IReadOnlyList<IncidentType>> GetIncidentTypesAsync();
        Task<IReadOnlyList<IncidentHeading>> GetIncidentHeadingsAsync();
        Task<IReadOnlyList<IncidentStatus>> GetIncidentStatusesAsync();
        Task<Incident> SubmitIncidentReportAsync(SubmitIncidentDto submitIncidentDto, string incidentReference);
        Task<IncidentAudit> SubmitIncidentAuditAsync(SubmitIncidentAuditDto submitIncidentAuditDto);
    }
}
