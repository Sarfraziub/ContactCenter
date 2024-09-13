using ContactCenter.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactCenter.Data
{
    public partial class Ticket
    {
        public Ticket()
        {
            Calls = new HashSet<Call>();
        }

        public Guid Id { get; set; }
        public string? ContactId { get; set; }
        public int StatusId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? CategoryId { get; set; }
        public string NotesJson { get; set; }
        public string? CreatorId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TypeId { get; set; }
        public Guid? AssigneeId { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public string LocationAddress { get; set; }
        [NotMapped]
        public string StatusName { get; set; }
        public int TicketHeadingId { get; set; }
        public Lib.TicketStatus TicketStatus { get; set; }
        public virtual User Assignee { get; set; }
        public virtual TicketCategory Category { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ContactUser Creator { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<Call> Calls { get; set; }
        public virtual ICollection<TicketAudit> TicketAudits { get; set; }

    }
}
