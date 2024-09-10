using ContactCenter.Data;
using ContactCenter.Data.Entities;
using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;
using EDRSM.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EDRSM.API.Controllers
{
    public class CouncillorsController : BaseApiController
    {
        private readonly UserManager<EdrsmAppUser> _userManager;
        private readonly ICouncillorsRepository _councillorsRepository;
        private readonly IPhotoService _photoService;

        public CouncillorsController(
            UserManager<EdrsmAppUser> userManager,
            ICouncillorsRepository councillorsRepository,
            IPhotoService photoService
            )
        {
            _userManager = userManager;
            _councillorsRepository = councillorsRepository;
            _photoService = photoService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Councillor>>> GetCouncillors()
        {
            var councillors = await _councillorsRepository.GetAllCouncillorsAsync();
            return Ok(councillors);
        }


        [HttpPost]
        public async Task<ActionResult<Councillor>> CreateCouncillor([FromForm] CreateCouncillorDto createCouncillorDto)
        {
            string imageUrl = "";
            string cloudinaryPublicId = "";

            if (createCouncillorDto.Image != null)
            {
                var result = await _photoService.AddPhotoAsync(createCouncillorDto.Image);

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

            var createdCouncillor = await _councillorsRepository.AddCouncillorAsync(councillor);

            return CreatedAtAction(nameof(GetCouncillor), new { id = createdCouncillor.Id }, createdCouncillor);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Councillor>> GetCouncillor(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            var councillor = await _councillorsRepository.GetCouncillorByIdAsync(guid);
            if (councillor == null)
            {
                return NotFound();
            }

            return Ok(councillor);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCouncillor(string id, [FromForm] CreateCouncillorDto updateCouncillorDto)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            var councillor = await _councillorsRepository.GetCouncillorByIdAsync(guid);
            if (councillor == null)
            {
                return NotFound();
            }

            // Update fields
            councillor.Name = updateCouncillorDto.Name;
            councillor.ContactNumber = updateCouncillorDto.ContactNumber;
            councillor.Ward = updateCouncillorDto.Ward;

            // If a new image is provided, upload it and update the Image property
            if (updateCouncillorDto.Image != null)
            {

                if (!string.IsNullOrWhiteSpace(updateCouncillorDto.CloudinaryPublicId))
                {
                    await _photoService.DeletePhotoAsync(updateCouncillorDto.CloudinaryPublicId);
                }

                var result = await _photoService.AddPhotoAsync(updateCouncillorDto.Image);
                if (result.Error != null) return BadRequest(result.Error.Message);

                councillor.Image = result.SecureUrl.AbsoluteUri; // Update the image URL
                councillor.CloudinaryPublicId = result.PublicId;
            }

            var updated = await _councillorsRepository.UpdateCouncillorAsync(councillor);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCouncillor(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            var deleted = await _councillorsRepository.DeleteCouncillorAsync(guid);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
