using ContactCenter.Data;
using EDRSM.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EDRSM.API.Errors;
using EDRSM.API.Helpers;
using ContactCenter.Web.API;
using Microsoft.EntityFrameworkCore;
using ContactCenter.Data.Entities;

namespace EDRSM.API.Controllers
{
    public class RequestedPaymentPlanController : SysAPIController
    {
        private readonly UserManager<ContactUser> _userManager;

        public RequestedPaymentPlanController(
            UserManager<ContactUser> userManager
            )
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RequestedPaymentPlan>>> GetPlans()
        {
            var plan = await Db.RequestedPaymentPlans.ToListAsync();
            return Ok(plan);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestedPaymentPlan>> GetPlan(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid ID format.");
            }

            var plan = await Db.RequestedPaymentPlans.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            return Ok(plan);
        }

        [HttpPost]
        public async Task<ActionResult<RequestedPaymentPlan>> PostPlan(CreateRequestedPaymentPlanDto planDto)
        {
            // Find the user who is submitting the payment plan
            var user = await _userManager.FindByIdAsync(planDto.UserId);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            var paymentPlanReference = IncidentReferenceGenerator.GeneratePaymentPlanReference();

            var plan = new RequestedPaymentPlan
            {
                Id = Guid.NewGuid(),
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
                CellphoneNumber = user.PhoneNumber
            };

            await Db.RequestedPaymentPlans.AddAsync(plan);

            await Db.SaveChangesAsync();

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

            var existingPlan = await Db.RequestedPaymentPlans.FindAsync(guid);
            if (existingPlan == null)
            {
                return NotFound("Payment plan not found.");
            }

            existingPlan.ReviewStatus = plan.ReviewStatus;
            existingPlan.ReviewComment = plan.ReviewComment;
            existingPlan.RequestReviewedDate = DateTime.Now;
            existingPlan.AdminReviewerId = plan.AdminReviewerId;
            existingPlan.AdminReviewerName = plan.AdminReviewerName;
            existingPlan.AdminReviewerSurname = plan.AdminReviewerSurname;

            await Db.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlan(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format.");
            }

            var plan = await Db.RequestedPaymentPlans.FindAsync(guid);
            if (plan == null)
            {
                return NotFound("Payment plan not found.");
            }

            Db.RequestedPaymentPlans.Remove(plan);

            await Db.SaveChangesAsync();

            return NoContent();
        }

    }
}
