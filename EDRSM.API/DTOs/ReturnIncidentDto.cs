namespace EDRSM.API.DTOs
{
    public class ReturnIncidentDto
    {
        public int StatusId { get; set; }
        public string Reference { get; set; }
        public string TimePosted { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCoordinates { get; set; }
        public string ContactNumber { get; set; }
        public string UserId { get; set; }
        public string HeadingName { get; set; }
        public string TypeName { get; set; }
        public int IncidentTypeId { get; set; }
        public int IncidentHeadingId { get; set; }
        public int Id { get; set; }
    }
}
