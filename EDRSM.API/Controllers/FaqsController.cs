using ContactCenter.Data;
using ContactCenter.Data.Identity;
using EDRSM.API.DTOs;
using EDRSM.API.Implementation;
using EDRSM.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EDRSM.API.Controllers
{
    public class FaqsController : BaseApiController
    {
        private readonly IFaqRepository _faqRepository;
        private readonly UserManager<EdrsmAppUser> _userManager;

        public FaqsController(
            UserManager<EdrsmAppUser> userManager,
            IFaqRepository faqRepository
            )
        {
            _userManager = userManager;
            _faqRepository = faqRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Faq>>> GetFaqs()
        {
            var faqs = await _faqRepository.GetAllAsync();
            return Ok(faqs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> GetFaq(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid ID format.");
            }

            var faq = await _faqRepository.GetByIdAsync(guidId);
            if (faq == null)
            {
                return NotFound();
            }
            return Ok(faq);
        }

        [HttpPost]
        public async Task<ActionResult<Faq>> PostFaq(CreateFaqDto faqDto)
        {
            // Map the DTO to the entity
            var faq = new Faq
            {
                Id = Guid.NewGuid(), // Generate a new GUID
                Category = faqDto.Category,
                ByCategorySorter = faqDto.ByCategorySorter,
                Question = faqDto.Question,
                Answer = faqDto.Answer
            };

            var createdFaq = await _faqRepository.AddAsync(faq);
            return CreatedAtAction(nameof(GetFaq), new { id = faq.Id }, faq);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutFaq(string id, Faq faq)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            if (guid != faq.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var updated = await _faqRepository.UpdateAsync(faq);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFaq(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            var deleted = await _faqRepository.DeleteAsync(guid);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetUniqueCategories()
        {
            var categories = await _faqRepository.GetUniqueCategoriesAsync();
            return Ok(categories);
        }
    }
}
