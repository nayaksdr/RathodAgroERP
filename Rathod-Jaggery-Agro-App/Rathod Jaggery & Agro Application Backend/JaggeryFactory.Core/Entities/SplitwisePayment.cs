using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class SplitwisePayment
    {
        public int Id { get; set; }
        public int FromMemberId { get; set; }
        public int ToMemberId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public Member FromMember { get; set; }
        public Member ToMember { get; set; }
        public int? PaidById { get; set; }
        public Member PaidBy { get; set; }
    }
}
