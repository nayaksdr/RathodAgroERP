using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class JaggerySaleReadDto
    {
        public int Id { get; set; }
        public int? DealerId { get; set; }
        public string DealerName { get; set; }
        public int? LaborId { get; set; }
        public string LaborName { get; set; }
        public DateTime SaleDate { get; set; }
        public double QuantityInKg { get; set; }
        public double RatePerKg { get; set; }
        public double TotalAmount { get; set; }
        public double AdvancePaid { get; set; }
        public double RemainingAmount { get; set; }
        public string? ProofImage { get; set; }
    }
}
