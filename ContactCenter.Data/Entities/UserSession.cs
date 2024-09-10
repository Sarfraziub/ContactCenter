using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class UserSession
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string ClientIpAddress { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public int? AuthSchemeId { get; set; }

        public virtual User User { get; set; }
    }
}
