using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class LaborTypeRate
    {
        public int Id { get; set; }
        public int LaborTypeId { get; set; }       // FK
        public LaborType LaborType { get; set; }   // navigation property
        public string PaymentType { get; set; } = "Daily";//"Daily" | "TonBased"
        public decimal DailyRate { get; set; }       
        // 🔹 Add these (new if not already):
        public decimal? PerTonRate { get; set; }     // For Cane Breaker
        public decimal? PerProductionRate { get; set; } // For Jaggery Maker
        public DateTime EffectiveFrom { get; set; }
       
    }

}
