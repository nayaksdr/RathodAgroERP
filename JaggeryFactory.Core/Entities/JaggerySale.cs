using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class JaggerySale
    {
        public int Id { get; set; }
        public int DealerId { get; set; }

        public int? PaidById { get; set; }
        public decimal QuantityInKg { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal TotalAmount { get; set; } // Quantity * Rate
        public decimal AdvancePaid { get; set; }
        public decimal RemainingAmount { get; set; } // TotalAmount - AdvancePaid
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [NotMapped]
        public string SearchDealer { get; set; }
        public Dealer Dealer { get; set; } // Navigation          
        public Member? PaidBy { get; set; }
        public string? PaymentMode { get; set; } // Cash, UPI, Bank
        public string? ProofImage { get; set; }  // Path of uploaded image

        // public decimal UsedAdvance { get; set; } = 0m;

        // ✅ Optional linkage to Labor (for labor-based jaggery calculations)
        public int? LaborId { get; set; }   // ✅ add this line (nullable if optional)
        public Labor? Labor { get; set; }   // ✅ navigation property
        
        public ICollection<JaggerySaleShare> Shares { get; set; } = new List<JaggerySaleShare>();

    }

}
