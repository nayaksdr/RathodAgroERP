using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class JaggeryProduce
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ProducedDate { get; set; } = DateTime.UtcNow;

        [Required, StringLength(50)]
        public string? BatchNumber { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be > 0")]
        public decimal QuantityKg { get; set; }

        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Unit price can't be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // per kg

        [NotMapped]
        public decimal TotalCost => QuantityKg * UnitPrice; // computed client/server side

        // persisted TotalCost snapshot (optional)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCostSnapshot { get; set; }

        // link to Labor who produced it (optional FK)
        public int? ProducedByLaborId { get; set; }
        //public Labor ProducedBy { get; set; } // uncomment if Labor entity exists and navigation desired

        [StringLength(20)]
        public string? QualityGrade { get; set; } // e.g., A, B, C

        [StringLength(500)]
        public string? Notes { get; set; }

        // Track current stock (if you want aggregated stock per batch)
        public decimal StockKg { get; set; }
    }
}

