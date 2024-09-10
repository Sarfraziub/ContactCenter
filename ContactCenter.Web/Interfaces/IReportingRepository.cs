using ContactCenter.Data;
using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;

namespace EDRSM.API.Interfaces
{
    public interface IReportingRepository
    {
        Task<IReadOnlyList<TicketType>> GetIncidentTypesAsync();
        Task<IReadOnlyList<TicketHeading>> GetIncidentHeadingsAsync();
        Task<IReadOnlyList<TicketStatus>> GetIncidentStatusesAsync();
        Task<Ticket> SubmitIncidentReportAsync(SubmitTicketDto submitIncidentDto, string incidentReference);
        Task<TicketAudit> SubmitIncidentAuditAsync(SubmitIncidentAuditDto submitIncidentAuditDto);
    }
}
