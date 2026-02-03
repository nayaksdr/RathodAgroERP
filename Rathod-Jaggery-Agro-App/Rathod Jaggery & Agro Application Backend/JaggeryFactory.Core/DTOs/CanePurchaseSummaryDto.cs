using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class CanePurchaseSummaryDto
    {
        public int FarmerId { get; set; }
        public string FarmerName { get; set; } = "";
        public decimal TotalTons { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public string? CaneWeightImagePath { get; set; }
    }
}
