using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class AgentSession
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckoutTime { get; set; }

        public virtual Agent Agent { get; set; }
    }
}
