using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class CanePaymentSummaryVM
    {
        public int FarmerId { get; set; }
        public string FarmerName { get; set; }

        public decimal TotalPurchase { get; set; }
        public decimal TotalAdvance { get; set; }
        public decimal NetAmount { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaymentDate { get; set; }  // nullable to prevent crashes
        public string? PaymentMode { get; set; }
        public int? MemberId { get; set; }
        public string? MemberName { get; set; }
        public string? PaymentProofPath { get; set; }
        public decimal? CarryForward { get; set; }   // nullable for safety
        public decimal PaidAmount { get; set; } // ✅ new
        public decimal BalanceAmount { get; set; } // ✅ new        
        public string? PaymentStatus { get; set; }
    }

}
