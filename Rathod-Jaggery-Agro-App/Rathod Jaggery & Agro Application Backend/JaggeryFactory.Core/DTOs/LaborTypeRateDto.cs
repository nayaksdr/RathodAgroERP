using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class LaborTypeRateDto
    {
        public int Id { get; set; }

        public int LaborTypeId { get; set; }
        public string LaborTypeName { get; set; }   // 🔥 MUST HAVE
        public string PaymentType { get; set; } = "Daily";

        public decimal DailyRate { get; set; }

        public decimal? PerTonRate { get; set; }

        public decimal? PerProductionRate { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }
}
