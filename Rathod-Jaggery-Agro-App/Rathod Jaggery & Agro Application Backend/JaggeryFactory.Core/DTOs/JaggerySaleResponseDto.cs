using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class JaggerySaleResponseDto
    {
        public int Id { get; set; }
        public string DealerName { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal QuantityInKg { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AdvancePaid { get; set; }
        public decimal RemainingAmount { get; set; }
    }

}
