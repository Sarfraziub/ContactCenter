using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class TicketCategory
    {
        public TicketCategory()
        {
            InverseParent = new HashSet<TicketCategory>();
            Tickets = new HashSet<Ticket>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User Creator { get; set; }
        public virtual TicketCategory Parent { get; set; }
        public virtual ICollection<TicketCategory> InverseParent { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
