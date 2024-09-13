using System.ComponentModel.DataAnnotations;

namespace ContactCenter.Web.DTOs
{
    public class TicketStatusDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
    }
}
