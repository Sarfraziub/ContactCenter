using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public class TicketHeading
    {
        public int Id { get; set; }
        public int TicketTypeId { get; set; }
        public string HeadingName { get; set; }

        public TicketType TicketType { get; set; }
        public ICollection<Ticket> Incidents { get; set; }
    }
}
