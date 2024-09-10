using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCenter.Data.Entities
{
    public partial class UserExternalLogin
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string LoginId { get; set; }
        public string ProviderKey { get; set; }
        public int AuthSchemeId { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User User { get; set; }
    }
}
