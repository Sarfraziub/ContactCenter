using System.ComponentModel.DataAnnotations;

namespace EDRSM.API.DTOs
{
    public class EdrsmUserToReturnDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string MunicipalityAccountNumber { get; set; }
        public string CellphoneNumber { get; set; }
        public int PreferredContactMethodId { get; set; }
        public int IdentificationTypeId { get; set; }
        public int CountryOfOriginId { get; set; }
        public string IdentificationNumber { get; set; }
        public bool IsAdmin { get; set; }
        public string Ward { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
