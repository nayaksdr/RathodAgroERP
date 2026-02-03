using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class RecordPaymentDto
    {
        [Required]
        public int SaleId { get; set; }

        [Required]
        public int FromMemberId { get; set; }

        [Required]
        public int ToMemberId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMode { get; set; } = string.Empty;

        // Optional proof image (UPI / Bank)
        public IFormFile? ProofImage { get; set; }
    }
}

