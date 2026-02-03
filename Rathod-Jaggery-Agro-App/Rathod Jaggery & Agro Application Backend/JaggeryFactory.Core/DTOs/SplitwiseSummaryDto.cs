using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class SplitwiseSummaryDto
    {
        public IEnumerable<Member> Members { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
        public Dictionary<int, decimal> Balances { get; set; }
        public IEnumerable<(int from, int to, decimal amount)> Settlements { get; set; }
    }
}
