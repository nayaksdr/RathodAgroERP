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
        public decimal DailyRate { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }

}
