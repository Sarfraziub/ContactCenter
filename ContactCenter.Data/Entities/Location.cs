using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class Location
    {
        public Location()
        {
            Calls = new HashSet<Call>();
            Contacts = new HashSet<Contact>();
            InverseParent = new HashSet<Location>();
            Tickets = new HashSet<Ticket>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public Guid? ParentId { get; set; }
        public decimal? GeoLatitude { get; set; }
        public decimal? GeoLongitude { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User Creator { get; set; }
        public virtual Location Parent { get; set; }
        public virtual ICollection<Call> Calls { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Location> InverseParent { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
