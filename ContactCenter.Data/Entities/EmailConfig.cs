using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class EmailConfig
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int TargetId { get; set; }
        public string SenderId { get; set; }
        public string Username { get; set; }
        public string SenderDisplayName { get; set; }
        public string Hash { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User Creator { get; set; }
    }
}
