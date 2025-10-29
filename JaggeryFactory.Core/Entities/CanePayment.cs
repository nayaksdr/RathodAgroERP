using DocumentFormat.OpenXml.Bibliography;
using JaggeryAgro.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class CanePayment
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }

        [Column("FarmerId")]
        public int FarmerId { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }

        [NotMapped]
        public string FarmerName { get; set; }

        public decimal GrossAmount { get; set; } // Total purchases for the period
        [NotMapped]
        public decimal AdvanceAdjusted { get; set; } // Total advance consumed this payment
        public decimal NetAmount { get; set; } // Gross - Adjusted
        public decimal CarryForwardAdvance { get; set; } // Unused advance after this payment (sum Remaining)
        [NotMapped]
        public decimal AdvanceAmount { get; set; }
        public Farmer? Farmer { get; set; }

        public PaymentType PaymentType { get; set; }   // <--- Enum property

        // 🔹 New fields for payment tracking
        public bool IsPaid { get; set; } // true = Paid, false = Pending
        public string? PaymentMode { get; set; } // Cash / UPI / Bank
        public string? PaymentProofPath { get; set; } // uploaded file path

    }

}
