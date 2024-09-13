using System.ComponentModel.DataAnnotations;

namespace ContactCenter.Web.DTOs
{
    public class TicketTypeDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
}
