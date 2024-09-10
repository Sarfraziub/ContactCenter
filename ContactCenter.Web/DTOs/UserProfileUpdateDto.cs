namespace EDRSM.API.DTOs
{
    public class UserProfileUpdateDto
    {
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
        public string Ward { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CloudinaryPublicId { get; set; }
        public bool isAdmin { get; set; }
    }
}
