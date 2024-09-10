namespace EDRSM.API.DTOs
{
    public class SignupDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MunicipalityAccountNumber { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public int PreferredContactMethodId { get; set; }
        public int IdentificationTypeId { get; set; }
        public int CountryOfOriginId { get; set; }
        public string IdentificationNumber { get; set; }
        public bool AgreedToTerms { get; set; }
        public string Ward { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
