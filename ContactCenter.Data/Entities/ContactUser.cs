using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCenter.Data.Entities
{
    public class ContactUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DisplayName { get; set; }
        public string MunicipalityAccountNumber { get; set; }
        public int PreferredContactMethodId { get; set; }
        public int IdentificationTypeId { get; set; }
        public string IdentificationNumber { get; set; }
        public bool AgreedToTerms { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public int CountryOfOriginId { get; set; }
        public int Ward { get; set; }
        public bool IsAdmin { get; set; } = false;
        public virtual Country CountryOfOrigin { get; set; }
        public virtual Attachment ProfilePicture { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
