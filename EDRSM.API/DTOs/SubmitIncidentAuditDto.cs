namespace EDRSM.API.DTOs
{
    public class SubmitIncidentAuditDto
    {
        public int IncidentId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string UserId { get; set; }
        public string ShortSummary { get; set; }
        public string DetailedDescription { get; set; }
    }
}
