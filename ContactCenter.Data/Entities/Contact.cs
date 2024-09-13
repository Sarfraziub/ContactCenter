using ContactCenter.Data.Entities;
using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class Contact
  {
    public Contact()
    {
      Calls = new HashSet<Call>();
      Tickets = new HashSet<Ticket>();
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string DetailsJson { get; set; }
    public Guid? LocationId { get; set; }
    public virtual Location Location { get; set; }

    public Guid CreatorId { get; set; }
    public virtual User Creator { get; set; }

    public DateTime CreationDate { get; set; }
    public Guid ContactUserId { get; set; }
    public virtual ContactUser ContactUser { get; set; }

    public virtual ICollection<Call> Calls { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
  }
}
