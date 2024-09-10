using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class Councillor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Ward { get; set; }
        public string Image { get; set; }
        public string CloudinaryPublicId { get; set; }
    }
}
