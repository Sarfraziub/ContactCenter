using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class Faq
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public int ByCategorySorter { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
