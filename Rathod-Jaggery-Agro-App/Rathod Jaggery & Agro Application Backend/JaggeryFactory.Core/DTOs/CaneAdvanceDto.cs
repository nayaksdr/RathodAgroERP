using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class CaneAdvanceDto
    {
        public int Id { get; set; }
        public int FarmerId { get; set; }
        public int MemberId { get; set; }
        public DateTime AdvanceDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = "";
        public string? Remarks { get; set; }
        public string? ProofImage { get; set; }

    }
}
