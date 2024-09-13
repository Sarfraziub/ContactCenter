using ContactCenter.Data;
using ContactCenter.Data.Entities;
using ContactCenter.Web.API;
using EDRSM.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDRSM.API.Controllers
{
    public class FaqsController : SysAPIController
    {
        private readonly UserManager<ContactUser> _userManager;

        public FaqsController(
            UserManager<ContactUser> userManager
            )
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Faq>>> GetFaqs()
        {
            var faqs = await Db.Faqs.ToListAsync();
            return Ok(faqs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> GetFaq(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid ID format.");
            }

            var faq = await Db.Faqs.FindAsync(id);
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

            await Db.Faqs.AddAsync(faq);
            var createdFaq = await Db.SaveChangesAsync();
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

            var existingFaq = await Db.Faqs.FindAsync(faq.Id);
            if (existingFaq == null)
            {
                return NotFound();
            }

            existingFaq.Category = faq.Category;
            existingFaq.ByCategorySorter = faq.ByCategorySorter;
            existingFaq.Question = faq.Question;
            existingFaq.Answer = faq.Answer;

            await Db.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFaq(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            var faq = await Db.Faqs.FindAsync(guid);
            if (faq == null)
            {
                return NotFound();
            }

            Db.Faqs.Remove(faq);

            await Db.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetUniqueCategories()
        {
            var categories = await Db.Faqs
                .Select(f => f.Category)
                .Distinct()
                .ToListAsync();
            return Ok(categories);
        }
    }
}
