using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class CallCategory
    {
        public CallCategory()
        {
            Calls = new HashSet<Call>();
            InverseParent = new HashSet<CallCategory>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User Creator { get; set; }
        public virtual CallCategory Parent { get; set; }
        public virtual ICollection<Call> Calls { get; set; }
        public virtual ICollection<CallCategory> InverseParent { get; set; }
    }
}
