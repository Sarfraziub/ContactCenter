using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactCenter.Data;
using ContactCenter.Data.Entities;
using ContactCenter.Web.API;
using EDRSM.API.DTOs;
using EDRSM.API.Errors;
using EDRSM.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EDRSM.API.Controllers
{
    public class UserController : SysAPIController
    {
        private readonly UserManager<ContactUser> _userManager;
        private readonly Cloudinary _cloudinary;
        //public PhotoService(IOptions<CloudinarySettings> config)
        //{
        //    var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        //    _cloudinary = new Cloudinary(acc);
        //}
        public UserController(
            UserManager<ContactUser> userManager,
            IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
            _userManager = userManager;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IReadOnlyList<Country>>> GetCountries()
        {
            return Ok(await Db.Countries.ToListAsync());
        }

        //[HttpGet("identification-types")]
        //public async Task<ActionResult<IReadOnlyList<IdentificationType>>> GetIdentificationTypes()
        //{
        //    return Ok(await _userRepo.GetIdentificationTypesAsync());
        //}

        //[HttpGet("contact-methods")]
        //public async Task<ActionResult<IReadOnlyList<PreferredContactMethod>>> GetContactMethods()
        //{
        //    return Ok(await _userRepo.GetContactMethodsAsync());
        //}


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
            user.Ward = int.Parse(userProfileUpdateDto.Ward);
            user.Email = userProfileUpdateDto.Email;
            user.PhoneNumber = userProfileUpdateDto.CellphoneNumber;
            user.PreferredContactMethodId = userProfileUpdateDto.PreferredContactMethodId;
            user.IdentificationTypeId = userProfileUpdateDto.IdentificationTypeId;
            user.CountryOfOriginId = userProfileUpdateDto.CountryOfOriginId;
            user.IsAdmin = userProfileUpdateDto.isAdmin;

            // If a new image is provided, upload it and update the Image property
            if (profileImage != null)
            {
                if (!string.IsNullOrWhiteSpace(userProfileUpdateDto.CloudinaryPublicId))
                {
                    await DeletePhotoAsync(userProfileUpdateDto.CloudinaryPublicId);
                }

                var result = await AddPhotoAsync(profileImage);
                if (result.Error != null) return BadRequest(result.Error.Message);

                //user.ProfileImageUrl = result.SecureUrl.AbsoluteUri; // Update the image URL
               // user.CloudinaryPublicId = result.PublicId;
            }

            Db.Entry(user).State = EntityState.Modified;
            await Db.SaveChangesAsync();

            return NoContent();
        }
        private async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
        private async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                        .Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "da-net8"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }
    }
}
