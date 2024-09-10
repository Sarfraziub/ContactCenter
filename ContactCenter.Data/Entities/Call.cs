using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class Call
    {
        public Guid Id { get; set; }
        public string ContactId { get; set; }
        public Guid AgentId { get; set; }
        public int DirectionId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? TicketId { get; set; }
        public string NotesJson { get; set; }
        public string Extension { get; set; }
        public string CallerId { get; set; }

        public virtual Agent Agent { get; set; }
        public virtual CallCategory Category { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Location Location { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
