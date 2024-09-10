using System;
using System.Collections.Generic;

namespace ContactCenter.Data
{
    public partial class RequestedPaymentPlan
    {
        public Guid Id { get; set; }
        public string SelectedAccount { get; set; }
        public string ApplicationReference { get; set; }
        public decimal AmountDue { get; set; }
        public decimal DepositPercentage { get; set; }
        public string PaymentPlan { get; set; }
        public decimal ImpliedMonthlyPayment { get; set; }
        public decimal AmountPaidDown { get; set; }
        public string ReasonForPlan { get; set; } // Detailed reason for requesting the payment plan
        public DateTime RequestPostedDate { get; set; }
        public string UserId { get; set; } // User ID of the person requesting the plan
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MunicipalityAccountNumber { get; set; }
        public string CellphoneNumber { get; set; }
        public bool AgreeToTermsAndConditions { get; set; }
        public string ReviewStatus { get; set; } // Status of the payment plan request (e.g., Pending, Approved, Rejected)
        public string ReviewComment { get; set; }
        public DateTime RequestReviewedDate { get; set; }
        public string AdminReviewerId { get; set; }
        public string AdminReviewerName { get; set; }
        public string AdminReviewerSurname { get; set; }

    }
}
