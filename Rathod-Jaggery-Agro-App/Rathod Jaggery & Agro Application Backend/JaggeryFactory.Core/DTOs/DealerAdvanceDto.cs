using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class DealerAdvanceDto
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public int PaidById { get; set; }
        public string PaidByName { get; set; }        
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string? ProofImage { get; set; }
        public string? Remarks { get; set; }
    }
}
