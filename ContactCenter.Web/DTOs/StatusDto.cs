namespace ContactCenter.Web.DTOs
{
    public class StatusDto
    {
        public Guid TicketId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
    }
}
