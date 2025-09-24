using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class JaggerySale
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public decimal QuantityInKg { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal TotalAmount { get; set; } // Quantity * Rate
        public decimal AdvancePaid { get; set; }
        public decimal RemainingAmount { get; set; } // TotalAmount - AdvancePaid
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [NotMapped]
        public string SearchDealer { get; set; }
        public Dealer Dealer { get; set; } // Navigation
    }

}
