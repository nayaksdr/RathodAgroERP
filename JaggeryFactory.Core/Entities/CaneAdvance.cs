using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class CaneAdvance
    {
        public int Id { get; set; }
        public int FarmerId { get; set; }
        public DateTime AdvanceDate { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingAmount { get; set; } // = Amount on create; reduces on settlements
        public decimal AdvanceAdjusted { get; set; }
        public string? Remarks { get; set; }
        public decimal AdvanceAmount { get; set; }
        
        public bool IsClosed => RemainingAmount <= 0;
        public Farmer? Farmer { get; set; }
        // Navigation
       
    }
}
