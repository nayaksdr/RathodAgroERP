using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class ProduceCreateDto
    {
        public DateTime ProducedDate { get; set; }
        public string? BatchNumber { get; set; }
        public decimal QuantityKg { get; set; }
        public decimal UnitPrice { get; set; }
        public string? QualityGrade { get; set; }
        public string? Notes { get; set; }
    }
}
