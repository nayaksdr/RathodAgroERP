using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class PaymentPreviewVm
    {
        public Farmer Farmer { get; set; } = default!;
        public DateTime From { get; set; }
        public DateTime To { get; set; }


        public IEnumerable<CanePurchase> Purchases { get; set; } = Enumerable.Empty<CanePurchase>();
        public IEnumerable<CaneAdvance> AdvancesApplied { get; set; } = Enumerable.Empty<CaneAdvance>();


        public decimal GrossAmount { get; set; }
        public decimal AdvanceAdjusted { get; set; }
        public decimal NetAmount { get; set; }
        public decimal CarryForwardAdvance { get; set; }


        // After confirmation, this becomes CanePayment
    }
}
