namespace EDRSM.API.DTOs
{
    public class SubmitIncidentDto
    {
        public int StatusId { get; set; }
        public DateTime TimePosted { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCoordinates { get; set; }
        public string ContactNumber { get; set; }
        public string UserId { get; set; }
        public int IncidentHeadingId { get; set; }
        public string HeadingName { get; set; }
        public int IncidentTypeId { get; set; }
        public string TypeName { get; set; }
        public string StatusName { get; set; }
        public string ShortSummary { get; set; }
        public string DetailedDescription { get; set; }
        public bool AnonymousSubmission { get; set; }
    }
}
