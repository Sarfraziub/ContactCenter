using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class Query
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public string QueryText { get; set; }
        public int Status { get; set; }
        public Guid AdminId { get; set; }
        public string UserId { get; set; }
    }
}
