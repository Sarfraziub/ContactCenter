using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCenter.Data.Entities
{
    public partial class FaqCategory
    {
        public FaqCategory()
        {
            Faqs = new HashSet<Faq>();
            InverseParent = new HashSet<FaqCategory>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }

        public virtual FaqCategory Parent { get; set; }
        public virtual ICollection<Faq> Faqs { get; set; }
        public virtual ICollection<FaqCategory> InverseParent { get; set; }
    }
}
