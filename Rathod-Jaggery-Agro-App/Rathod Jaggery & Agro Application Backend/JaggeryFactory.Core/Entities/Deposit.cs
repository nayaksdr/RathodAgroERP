using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class Deposit
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DepositDate { get; set; }

        public Labor? Labor { get; set; }
    }
}
