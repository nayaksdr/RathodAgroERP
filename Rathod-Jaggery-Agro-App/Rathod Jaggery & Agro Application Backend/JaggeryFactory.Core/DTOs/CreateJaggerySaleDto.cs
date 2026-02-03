using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class CreateJaggerySaleDto
    {
        public int DealerId { get; set; }
        public int LaborId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal QuantityInKg { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal AdvancePaid { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public int? PaidById { get; set; }
    }

}
