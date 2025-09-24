using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class CanePurchase
    {
        public int Id { get; set; }

        [Required]
        public int FarmerId { get; set; }
        public Farmer Farmer { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        // Quantity in quintals or tons — use the unit your factory uses
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal QuantityTon { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RatePerTon { get; set; } // price per ton (or per quintal)

        [NotMapped]
        public decimal TotalAmount => QuantityTon * RatePerTon;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmountSnapshot { get; set; } // persisted snapshot

        [StringLength(100)]
        public string? PaymentStatus { get; set; } // e.g., Pending, Paid

        [StringLength(500)]
        public string? Notes { get; set; } 

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public string FarmerName { get; set; } // ✅ Add this property
        

    }
}
