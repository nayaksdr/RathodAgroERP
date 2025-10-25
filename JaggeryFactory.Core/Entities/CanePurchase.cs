using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JaggeryAgro.Core.Entities
{
    public class CanePurchase
    {
        public int Id { get; set; }

        [Required]
        public int FarmerId { get; set; }
        public Farmer? Farmer { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        // ✅ Cane quantity (in tons)
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityTon { get; set; }

        // ✅ Rate per ton
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RatePerTon { get; set; }

        // ✅ Computed amount (not stored)
        [NotMapped]
        public decimal TotalAmount => QuantityTon * RatePerTon;

        // ✅ Snapshot (persisted)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmountSnapshot { get; set; }

        [StringLength(100)]
        public string? PaymentStatus { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public string? CaneWeightImagePath { get; set; }

        // ✅ Navigation to payments (if any)
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // ✅ Farmer name (nullable)
        [StringLength(150)]
        public string? FarmerName { get; set; }

        // ✅ Optional linkage to Labor (for labor-based jaggery calculations)
        public int? LaborId { get; set; }
        public Labor? Labor { get; set; }
    }
}
