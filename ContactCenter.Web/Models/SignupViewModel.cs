using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactCenter.Web.Models
{
    public class SignupViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string MunicipalityAccountNumber { get; set; }
        [Required]
        public string CellphoneNumber { get; set; }
        [Required]
        public string IdentificationNumber { get; set; }
        [Required]
        public int IdentificationTypeId { get; set; }
        [Required]
        public int PreferredContactMethodId { get; set; }
        [Required]
        public bool AgreedToTerms { get; set; }
        [Required]
        public int CountryOfOriginId { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        public List<SelectListItem> IdentificationTypes { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}
