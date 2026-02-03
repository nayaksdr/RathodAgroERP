using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class AdvancePaymentDto
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public string LaborName { get; set; } = string.Empty;

        public string LaborType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DateGiven { get; set; }
        public string? Remarks { get; set; }
       

    }
}
