using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class PaymentDto
    {
        public int FromMemberId { get; set; }
        public int ToMemberId { get; set; }
        public decimal Amount { get; set; }
    }
}
