using System;
using System.Collections.Generic;
using System.Text;

namespace ContactCenter.Lib.DataSync
{
    public class ICall
    {
        public Guid Id { get;set; }
        public string ContactId { get; set; }
        public string Direction { get; set; }
        public int Duration { get; set; }
        public string AgentId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
