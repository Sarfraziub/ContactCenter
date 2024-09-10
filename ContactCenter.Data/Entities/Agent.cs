using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class Agent
    {
        public Agent()
        {
            AgentSessions = new HashSet<AgentSession>();
            Calls = new HashSet<Call>();
            Tickets = new HashSet<Ticket>();
        }

        public Guid Id { get; set; }
        public string Extension { get; set; }
        public int StatusId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User Creator { get; set; }
        public virtual ICollection<AgentSession> AgentSessions { get; set; }
        public virtual ICollection<Call> Calls { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
