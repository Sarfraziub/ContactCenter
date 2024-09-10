using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;
using EDRSM.API.Errors;
using EDRSM.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wCyber.Helpers.Identity;

namespace EDRSM.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ContactUser> _userManager;
        private readonly SignInManager<ContactUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;

        public AccountController(
            ITokenService tokenService,
            UserManager<ContactUser> userManager,
            SignInManager<ContactUser> signInManager,
            IEmailSender emailSender
            )
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        [HttpGet("current-user")]
        public async Task<ActionResult<EdrsmUserToReturnDto>> GetCurrentUser([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return new EdrsmUserToReturnDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                IdentificationTypeId = user.IdentificationTypeId,
                IdentificationNumber = user.IdentificationNumber,
                PreferredContactMethodId = user.PreferredContactMethodId,
                CellphoneNumber = user.PhoneNumber,
                Ward = user.Ward.ToString(),
                CountryOfOriginId = user.CountryOfOriginId,
                IsAdmin = user.IsAdmin,
              //  ProfileImageUrl = user.ProfileImageUrl,
                MunicipalityAccountNumber = user.MunicipalityAccountNumber,
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<Boolean>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }


        [HttpPost("signup")]
        public async Task<ActionResult<UserLoginDto>> Signup(SignupDto signupDto)
        {
            // Check if Password and ConfirmPassword match
            if (signupDto.Password != signupDto.ConfirmPassword)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    {"Password and Confirm Password do not match"}
                });
            }

            if (CheckEmailExistsAsync(signupDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                {"Email address is in use"}
                });
            }

            var user = new ContactUser
            {
                DisplayName = signupDto.Name,
                Email = signupDto.Email,
                UserName = signupDto.Email,
                Name = signupDto.Name,
                Surname = signupDto.Surname,
                PhoneNumber = signupDto.CellphoneNumber,
                PreferredContactMethodId = signupDto.PreferredContactMethodId,
                IdentificationTypeId = signupDto.IdentificationTypeId,
                IdentificationNumber = signupDto.IdentificationNumber,
                MunicipalityAccountNumber = signupDto.MunicipalityAccountNumber,
                AgreedToTerms = signupDto.AgreedToTerms,
                Ward = int.Parse(signupDto.Ward),
                CountryOfOriginId = signupDto.CountryOfOriginId,
                IsAdmin = signupDto.IsAdmin,
            };

            var result = await _userManager.CreateAsync(user, signupDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserLoginDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                IsAdmin = user.IsAdmin,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserLoginDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserLoginDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName ?? user.Name,
                IsAdmin = user.IsAdmin,
                Token = _tokenService.CreateToken(user)
            };

        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Set your Flutter app's domain here
            var appDomain = model.AppDomain;

            // Create the reset password URL for the Flutter app
            var callbackUrl = $"{appDomain}/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";

            // Send the email using SendGrid
            await _emailSender.SendEmailAsync(user.Email, "Reset Password",
                $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");
            return Ok("Password reset email sent.");
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password reset successful.");
        }
    }
}
