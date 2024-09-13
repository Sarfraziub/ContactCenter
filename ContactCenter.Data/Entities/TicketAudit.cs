using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContactCenter.Data
{
    public class TicketAudit
    {
        public int Id { get; set; }
        public Guid TicketId { get; set; }
        public Lib.TicketStatus StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime StatusChangeTime { get; set; }
        public string UserId { get; set; }
        public string NameOfUpdater { get; set; }
        public string SurnameOfUpdater { get; set; }
        // public string ShortSummary { get; set; }
        public string Description { get; set; }

       
        [JsonIgnore]  // Prevents cyclical reference during serialization
        public virtual Ticket Ticket { get; set; }
    }
}
