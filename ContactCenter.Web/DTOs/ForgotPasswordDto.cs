﻿namespace EDRSM.API.DTOs
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public string AppDomain { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
