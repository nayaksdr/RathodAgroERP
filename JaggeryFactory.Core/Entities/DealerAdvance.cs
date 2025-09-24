using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class DealerAdvance
    {
        public int Id { get; set; }

        public int DealerId { get; set; }
        public Dealer Dealer { get; set; }

        public decimal Amount { get; set; }         // आगाऊ रक्कम (₹)
        public DateTime AdvanceDate { get; set; } = DateTime.Now;
        public string Note { get; set; }
    }
}
