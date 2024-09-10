using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wCyber.Helpers.Identity;

namespace ContactCenter.Data
{
    partial class EmailConfig
    {
        [NotMapped]
        public EmailConfigTarget Target
        {
            get => (EmailConfigTarget)TargetId;
            set => TargetId = (int)value;
        }

        [NotMapped]
        public string Password { get; set; }

        public string ComputeHash() => Hash = Password?.GetHash(Id.ToString());
        public string DecodePassword() => Hash?.GetPassword(Id.ToString());
    }
}
