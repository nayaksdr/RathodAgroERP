using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class ExpenseCreateDto
    {
        public int PaidById { get; set; }
        public decimal Amount { get; set; }
        public int? ExpenseTypeId { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public IFormFile? ProofImage { get; set; }
    }
}
