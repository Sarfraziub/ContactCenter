namespace EDRSM.API.DTOs
{
    public class CreateRequestedPaymentPlanDto
    {
        public string SelectedAccount { get; set; }
        public decimal AmountDue { get; set; }
        public decimal DepositPercentage { get; set; }
        public string PaymentPlan { get; set; }
        public decimal ImpliedMonthlyPayment { get; set; }
        public decimal AmountPaidDown { get; set; }
        public string ReasonForPlan { get; set; }
        public string UserId { get; set; }
        public string ReviewStatus { get; set; } // Status of the payment plan request (e.g., Pending, Approved, Rejected)
        public bool AgreeToTermsAndConditions { get; set; }
    }
}
