using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }

        public ICollection<TicketAudit> TicketAudits { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
