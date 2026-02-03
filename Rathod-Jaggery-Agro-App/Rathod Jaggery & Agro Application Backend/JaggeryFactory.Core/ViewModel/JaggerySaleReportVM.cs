using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class JaggerySaleReportVM
    {
        public int? DealerId { get; set; }
        public string DealerName { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public List<JaggerySale> Sales { get; set; } = new();
        public decimal TotalQuantityKg => Sales.Sum(s => s.QuantityInKg);
        public decimal TotalAmount => Sales.Sum(s => s.TotalAmount);
        public decimal TotalAdvance => Sales.Sum(s => s.AdvancePaid);
        public decimal TotalRemaining => Sales.Sum(s => s.RemainingAmount);
    }

    public class DealerBalanceVM
    {
        public int DealerId { get; set; }
        public string DealerName { get; set; }

        public List<JaggerySale> Sales { get; set; } = new();
        public decimal TotalSold => Sales.Sum(s => s.TotalAmount);
        public decimal TotalAdvance => Sales.Sum(s => s.AdvancePaid);
        public decimal Balance => TotalSold - TotalAdvance;
    }

}
