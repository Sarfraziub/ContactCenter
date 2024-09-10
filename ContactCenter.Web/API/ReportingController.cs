using ContactCenter.Data;
using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;
using EDRSM.API.Errors;
using EDRSM.API.Helpers;
using EDRSM.API.Implementation;
using EDRSM.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EDRSM.API.Controllers
{
    public class ReportingController : BaseApiController
    {
        private readonly UserManager<ContactUser> _userManager;
        private readonly IReportingRepository _reportingRepo;

        public ReportingController(
            UserManager<ContactUser> userManager,
            IReportingRepository reportingRepo
            )
        {
            _userManager = userManager;
            _reportingRepo = reportingRepo;
        }

        [HttpGet("incident-types")]
        public async Task<ActionResult<IReadOnlyList<TicketType>>> GeIncidentTypes()
        {
            return Ok(await _reportingRepo.GetIncidentTypesAsync());
        }

        [HttpGet("incident-headings")]
        public async Task<ActionResult<IReadOnlyList<TicketHeading>>> GeIncidentHeadings()
        {
            return Ok(await _reportingRepo.GetIncidentHeadingsAsync());
        }

        [HttpGet("incident-statuses")]
        public async Task<ActionResult<IReadOnlyList<TicketStatus>>> GetIncidentStatuses()
        {
            return Ok(await _reportingRepo.GetIncidentStatusesAsync());
        }

        [HttpPost("submit-incident")]
        public async Task<ActionResult<ReturnTicketDto>> SubmitIncidentReport([FromBody] SubmitTicketDto submitIncidentDto)
        {
            var user = await _userManager.FindByIdAsync(submitIncidentDto.CreatorId);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            var incidentReference = IncidentReferenceGenerator.GenerateIncidentReference();

            var incident = await _reportingRepo.SubmitIncidentReportAsync(submitIncidentDto, incidentReference);

            if (incident == null)
            {
                return BadRequest(new ApiResponse(400, "Failed to submit incident report"));
            }

            var returnIncidentDto = new ReturnTicketDto
            {
                Id = incident.Id,
                Reference = incidentReference,
                StatusId = incident.StatusId,
                //TimePosted = incident.TimePosted.ToLongDateString(),
                //LocationAddress = incident.LocationAddress,
                //LocationCoordinates = incident.LocationCoordinates,
                //ContactNumber = incident.ContactNumber,
                //UserId = incident.UserId,
                //IncidentHeadingId = incident.IncidentHeadingId,
                //HeadingName = incident.HeadingName,
                //IncidentTypeId = incident.IncidentTypeId,
                //TypeName = incident.TypeName
            };

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

            var incidentAudit = await _reportingRepo.SubmitIncidentAuditAsync(submitIncidentAuditDto);

            if (incidentAudit == null)
            {
                return BadRequest(new ApiResponse(400, "Failed to submit incident audit"));
            }

            return Ok(incidentAudit);
        }

    }
}
