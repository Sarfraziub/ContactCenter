using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wCyber.Helpers.Identity.Auth;

namespace ContactCenter.Data
{
    partial class User : ISysUser
    {
        [NotMapped]
        public UserRole Role
        {
            get => (UserRole)RoleId;
            set => RoleId = (int)value;
        }

        [NotMapped]
        public string Initials => (!Name.Trim().Contains(' ', StringComparison.CurrentCulture) ?
         Name[..Math.Min(2, Name.Length)] :
         new string(Name.Trim().Split(" ").Select(c => c[0]).ToArray())).ToUpper();

        [NotMapped]
        public bool IsMobileConfirmed { get; set; }

        [NotMapped]
        public string PictureUrl { get; set; }

        [NotMapped]
        public string Extension { get; set; }

        [NotMapped]
        public bool IsAgent => Role == UserRole.AGENT;

    }
}
