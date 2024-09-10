using System;
using System.Collections.Generic;
using System.Text;

namespace ContactCenter.Lib
{
    public enum TicketStatus : int
    {
        PENDING = 1,
        IN_PROGRESS = 2,
        RESOLVED = 3,
        CANCELLED = 4,
    }
}
