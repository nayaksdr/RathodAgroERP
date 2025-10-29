using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class LaborPayment
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public string LaborName { get; set; }
        public string LaborTypeName { get; set; } = ""; // <-- add this
        public DateTime PaymentDate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal AdvanceAdjusted { get; set; }
        public decimal NetAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int AttendanceDays { get; set; }
        public decimal rate { get; set; }
        public decimal? RatePerTon { get; set; }
        public decimal? RatePerUnit { get; set; }
        public decimal? TotalJaggeryQty { get; set; }
        public decimal? TotalTons { get; set; }
        public decimal WorkAmount { get; set; }
    }
}
