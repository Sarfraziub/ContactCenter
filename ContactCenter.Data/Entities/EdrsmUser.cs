using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class EdrsmUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string MunicipalityAccountNumber { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public int PreferredContactMethodId { get; set; }
        public int IdentificationTypeId { get; set; }
        public string IdentificationNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool AgreedToTerms { get; set; }
        public bool IsAdmin { get; set; }
        public string Ward { get; set; }
        public int CountryOfOriginId { get; set; }
        public string Id { get; set; }
    }
}
