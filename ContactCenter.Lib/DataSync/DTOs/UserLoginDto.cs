namespace EDRSM.API.DTOs
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public bool IsAdmin { get; set; }
    }
}
