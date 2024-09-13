using EDRSM.API.DTOs;
using EDRSM.API.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wCyber.Helpers.Identity;
using ContactCenter.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;
using ContactCenter.Lib;
using ContactCenter.Data.Entities;

namespace EDRSM.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly UserManager<ContactUser> _contactUserManager;
        private readonly SignInManager<ContactUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly CCDbContext _ccdbContext;

        public AccountController(
            //ITokenService tokenService,
            UserManager<User> userManager,
            UserManager<ContactUser> contactUserManager,
            SignInManager<ContactUser> signInManager,
            IConfiguration config,
            CCDbContext ccdbContext
            )
        {
            _userManager = userManager;
            _ccdbContext = ccdbContext;
            _contactUserManager = contactUserManager;
            _signInManager = signInManager;
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }


        [HttpGet("current-user")]
        public async Task<ActionResult<EdrsmUserToReturnDto>> GetCurrentUser([FromQuery] string email)
        {
            var user = await _contactUserManager.FindByEmailAsync(email);

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
            return await _contactUserManager.FindByEmailAsync(email) != null;
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
            var _user = new User
            {
                Id = Guid.NewGuid(),
                Name = signupDto.Name,
                Email = signupDto.Email.ToLower().Trim(),
                LoginId = signupDto.Email.ToLower().Trim(),
                CreationDate = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true,
                IsEmailConfirmed = true,
                Role = UserRole.CONTACT_USER,
                ActivationDate = DateTime.Now
            };
            _ccdbContext.Users.Add(_user);
            _user.PasswordHash = _userManager.PasswordHasher.HashPassword(_user, signupDto.Password);
            _ccdbContext.SaveChanges();
            user.UserId = _user.Id;
            var result = await _contactUserManager.CreateAsync(user, signupDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserLoginDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                IsAdmin = user.IsAdmin,
                Token = CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserLoginDto>> Login(LoginDto loginDto)
        {
            var user = await _contactUserManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserLoginDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName ?? user.Name,
                IsAdmin = user.IsAdmin,
                Token = CreateToken(user)
            };

        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var user = await _contactUserManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");

            var token = await _contactUserManager.GeneratePasswordResetTokenAsync(user);

            // Set your Flutter app's domain here
            var appDomain = model.AppDomain;

            // Create the reset password URL for the Flutter app
            var callbackUrl = $"{appDomain}/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";

            // Send the email using SendGrid
            await SendEmailAsync(user.Email, "Reset Password",
                $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");
            return Ok("Password reset email sent.");
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _contactUserManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");

            var result = await _contactUserManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password reset successful.");
        }
        private string CreateToken(ContactUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName ?? user.Name)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sandboxinnovative2023@gmail.com", "DC39 Web App");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            await client.SendEmailAsync(msg);
        }
    }
}
