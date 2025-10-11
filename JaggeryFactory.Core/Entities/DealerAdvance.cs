using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class DealerAdvance
    {
        public int Id { get; set; }

        public int DealerId { get; set; }
        public Dealer Dealer { get; set; }
        public int? PaidById { get; set; }
        public Member? PaidBy { get; set; }
        public string? PaymentMode { get; set; } // Cash, UPI, Bank
        public string? ProofImage { get; set; }  // Path of uploaded image
        public decimal Amount { get; set; }         // आगाऊ रक्कम (₹)
        public DateTime AdvanceDate { get; set; } = DateTime.Now;
        public string Note { get; set; }
    }
}
