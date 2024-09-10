using ContactCenter.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCenter.Data.Entities
{
    public partial class Attachment
    {
        public Attachment()
        {
            ContactUsers = new HashSet<ContactUser>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public Guid? UniqueId { get; set; }
        public string Container { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<ContactUser> ContactUsers { get; set; }
    }
}
