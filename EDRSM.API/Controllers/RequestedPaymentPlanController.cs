using ContactCenter.Data.Identity;
using ContactCenter.Data;
using EDRSM.API.DTOs;
using EDRSM.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EDRSM.API.Errors;
using EDRSM.API.Helpers;

namespace EDRSM.API.Controllers
{
    public class RequestedPaymentPlanController : BaseApiController
    {
        private readonly IRequestedPaymentPlanRepository _requestedPaymentPlanRepository;
        private readonly UserManager<EdrsmAppUser> _userManager;

        public RequestedPaymentPlanController(
            UserManager<EdrsmAppUser> userManager,
            IRequestedPaymentPlanRepository requestedPaymentPlanRepository
            )
        {
            _userManager = userManager;
            _requestedPaymentPlanRepository = requestedPaymentPlanRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RequestedPaymentPlan>>> GetPlans()
        {
            var plan = await _requestedPaymentPlanRepository.GetAllRequestedPaymentPlansAsync();
            return Ok(plan);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestedPaymentPlan>> GetPlan(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid ID format.");
            }

            var plan = await _requestedPaymentPlanRepository.GetRequestedPaymentPlanByIdAsync(guidId);
            if (plan == null)
            {
                return NotFound();
            }
            return Ok(plan);
        }

        [HttpPost]
        public async Task<ActionResult<RequestedPaymentPlan>> PostPlan(CreateRequestedPaymentPlanDto planDto)
        {
            var user = await _userManager.FindByIdAsync(planDto.UserId);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            var paymentPlanReference = IncidentReferenceGenerator.GeneratePaymentPlanReference();
            // Map the DTO to the entity
            var plan = new RequestedPaymentPlan
            {
                Id = Guid.NewGuid(), // Generate a new GUID
                ApplicationReference = paymentPlanReference,
                SelectedAccount = planDto.SelectedAccount,
                AmountDue = planDto.AmountDue,
                DepositPercentage = planDto.DepositPercentage,
                PaymentPlan = planDto.PaymentPlan,
                ImpliedMonthlyPayment = planDto.ImpliedMonthlyPayment,
                AmountPaidDown = planDto.AmountPaidDown,
                ReasonForPlan = planDto.ReasonForPlan,
                RequestPostedDate = DateTime.Now,
                UserId = planDto.UserId,
                Name = user.Name,
                Surname = user.Surname,
                MunicipalityAccountNumber = user.MunicipalityAccountNumber,
                ReviewStatus = planDto.ReviewStatus,
                AgreeToTermsAndConditions = planDto.AgreeToTermsAndConditions,
                CellphoneNumber = user.CellphoneNumber
            };

            var createdPlan = await _requestedPaymentPlanRepository.CreateRequestedPaymentPlanAsync(plan);
            return CreatedAtAction(nameof(GetPlan), new { id = plan.Id }, plan);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePlan(string id, RequestedPaymentPlan plan)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            if (guid != plan.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var updated = await _requestedPaymentPlanRepository.UpdateRequestedPaymentPlanAsync(plan);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlan(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            var deleted = await _requestedPaymentPlanRepository.DeleteRequestedPaymentPlanAsync(guid);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
