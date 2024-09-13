using System;

namespace EDRSM.API.DTOs
{
    public class SubmitIncidentAuditDto
    {
        public Guid TicketId { get; set; }
        public int StatusId { get; set; }
        public string NotesJson { get; set; }
        public string StatusName { get; set; }
        public string UserId { get; set; }
        // public string ShortSummary { get; set; }
        public string Description { get; set; }


        public string UpdatedBy { get; set; }
    }
}
