using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public class TicketType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public ICollection<TicketHeading> TicketHeadings { get; set; }
    }
}
    