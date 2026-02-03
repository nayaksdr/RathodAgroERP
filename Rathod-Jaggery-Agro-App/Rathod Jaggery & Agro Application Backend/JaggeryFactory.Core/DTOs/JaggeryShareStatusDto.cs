using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class JaggeryShareStatusDto
    {
        public int ShareId { get; set; }
        public int SaleId { get; set; }

        public string MemberName { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }

        public decimal ShareAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }

        public string Status { get; set; } = string.Empty;
        public string? PaymentMode { get; set; }
        public string? ProofImage { get; set; }
    }
}
