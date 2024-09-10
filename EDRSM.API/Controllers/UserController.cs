using ContactCenter.Data;
using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;
using EDRSM.API.Errors;
using EDRSM.API.Implementation;
using EDRSM.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EDRSM.API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly UserManager<EdrsmAppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUserOperationsRepository _userRepo;
        private readonly IPhotoService _photoService;

        public UserController(
            UserManager<EdrsmAppUser> userManager,
            ITokenService tokenService,
            IUserOperationsRepository userRepo,
            IPhotoService photoService
            )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _userRepo = userRepo;
            _photoService = photoService;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IReadOnlyList<Country>>> GetCountries()
        {
            return Ok(await _userRepo.GetCountryDetailsAsync());
        }

        [HttpGet("identification-types")]
        public async Task<ActionResult<IReadOnlyList<IdentificationType>>> GetIdentificationTypes()
        {
            return Ok(await _userRepo.GetIdentificationTypesAsync());
        }

        [HttpGet("contact-methods")]
        public async Task<ActionResult<IReadOnlyList<PreferredContactMethod>>> GetContactMethods()
        {
            return Ok(await _userRepo.GetContactMethodsAsync());
        }


        [HttpPut("update-profile")]
        public async Task<ActionResult> UpdateUserProfile([FromForm] UserProfileUpdateDto userProfileUpdateDto, IFormFile? profileImage)
        {
            var user = await _userManager.FindByEmailAsync(userProfileUpdateDto.Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            user.DisplayName = userProfileUpdateDto.DisplayName;
            user.Name = userProfileUpdateDto.Name;
            user.Surname = userProfileUpdateDto.Surname;
            user.IdentificationNumber = userProfileUpdateDto.IdentificationNumber;
            user.MunicipalityAccountNumber = userProfileUpdateDto.MunicipalityAccountNumber;
            user.Ward = userProfileUpdateDto.Ward;
            user.Email = userProfileUpdateDto.Email;
            user.CellphoneNumber = userProfileUpdateDto.CellphoneNumber;
            user.PreferredContactMethodId = userProfileUpdateDto.PreferredContactMethodId;
            user.IdentificationTypeId = userProfileUpdateDto.IdentificationTypeId;
            user.CountryOfOriginId = userProfileUpdateDto.CountryOfOriginId;
            user.IsAdmin = userProfileUpdateDto.isAdmin;

            // If a new image is provided, upload it and update the Image property
            if (profileImage != null)
            {
                if (!string.IsNullOrWhiteSpace(userProfileUpdateDto.CloudinaryPublicId))
                {
                    await _photoService.DeletePhotoAsync(userProfileUpdateDto.CloudinaryPublicId);
                }

                var result = await _photoService.AddPhotoAsync(profileImage);
                if (result.Error != null) return BadRequest(result.Error.Message);

                user.ProfileImageUrl = result.SecureUrl.AbsoluteUri; // Update the image URL
                user.CloudinaryPublicId = result.PublicId;
            }

            await _userRepo.UpdateUserProfile(user);

            return NoContent();
        }
    }
}
