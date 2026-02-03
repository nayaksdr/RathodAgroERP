using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class PaymentGenerateRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PaymentMethod { get; set; } = "";
    }
}
