using ContactCenter.Data.Entities;
using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class Country
    {
        //public Country()
        //{
        //    ContactUsers = new HashSet<ContactUser>();
        //}

        public string Name { get; set; }
        public int Id { get; set; }
        public string DialingCode { get; set; }
        public string CountryCode { get; set; }

        public virtual ICollection<ContactUser> ContactUsers { get; set; }
    }
}
