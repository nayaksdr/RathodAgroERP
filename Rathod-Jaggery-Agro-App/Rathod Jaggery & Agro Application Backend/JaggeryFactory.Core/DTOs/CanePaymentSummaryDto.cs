using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class CanePaymentSummaryDto
    {
        public int FarmerId { get; set; }
        public string FarmerName { get; set; } = "";
        public decimal TotalPurchase { get; set; }
        public decimal TotalAdvance { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal CarryForward { get; set; }
        public string PaymentStatus { get; set; } = "";
        public bool IsPaid { get; set; }       
        public string PaymentMode { get; set; } = "";
        public string MemberName { get; set; } = "";
        public string PaymentProofPath { get; set; } = "";
    }
}
