using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactCenter.Data;
using ContactCenter.Data.Entities;
using ContactCenter.Web.API;
using EDRSM.API.DTOs;
using EDRSM.API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDRSM.API.Controllers
{
    public class CouncillorsController : SysAPIController
    {
        private readonly UserManager<ContactUser> _userManager;
        private readonly Cloudinary _cloudinary;
        public CouncillorsController(
            UserManager<ContactUser> userManager, IOptions<CloudinarySettings> config)
        {
            _userManager = userManager; 
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Councillor>>> GetCouncillors()
        {
            var councillors = await Db.Councillors.ToListAsync();
            return Ok(councillors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Councillor>> GetCouncillor(Guid id)
        {
            var councillor = await Db.Councillors
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (councillor == null)
            {
                return NotFound();
            }

            return Ok(councillor);
        }

        [HttpPost]
        public async Task<ActionResult<Councillor>> CreateCouncillor([FromForm] CreateCouncillorDto createCouncillorDto)
        {
            string imageUrl = "";
            string cloudinaryPublicId = "";

            if (createCouncillorDto.Image != null)
            {
                var result = await AddPhotoAsync(createCouncillorDto.Image);

                if (result.Error != null) return BadRequest(result.Error.Message);

                imageUrl = result.SecureUrl.AbsoluteUri;
                cloudinaryPublicId = result.PublicId;
            }

            var councillor = new Councillor
            {
                Id = Guid.NewGuid(),
                Name = createCouncillorDto.Name,
                ContactNumber = createCouncillorDto.ContactNumber,
                Ward = createCouncillorDto.Ward,
                Image = imageUrl,
                CloudinaryPublicId = cloudinaryPublicId
            };

            Db.Councillors.Add(councillor);
            await Db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCouncillor), new { id = councillor.Id }, councillor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCouncillor(Guid id, [FromForm] CreateCouncillorDto updateCouncillorDto)
        {
            var councillor = await Db.Councillors.FindAsync(id);
            if (councillor == null)
            {
                return NotFound();
            }

            councillor.Name = updateCouncillorDto.Name;
            councillor.ContactNumber = updateCouncillorDto.ContactNumber;
            councillor.Ward = updateCouncillorDto.Ward;

            if (updateCouncillorDto.Image != null)
            {
                if (!string.IsNullOrWhiteSpace(councillor.CloudinaryPublicId))
                {
                    await DeletePhotoAsync(councillor.CloudinaryPublicId);
                }

                var result = await AddPhotoAsync(updateCouncillorDto.Image);
                if (result.Error != null) return BadRequest(result.Error.Message);

                councillor.Image = result.SecureUrl.AbsoluteUri;
                councillor.CloudinaryPublicId = result.PublicId;
            }

            Db.Councillors.Update(councillor);
            await Db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCouncillor(Guid id)
        {
            var councillor = await Db.Councillors.FindAsync(id);
            if (councillor == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(councillor.CloudinaryPublicId))
            {
                await DeletePhotoAsync(councillor.CloudinaryPublicId);
            }

            Db.Councillors.Remove(councillor);
            await Db.SaveChangesAsync();

            return NoContent();
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
        private async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
