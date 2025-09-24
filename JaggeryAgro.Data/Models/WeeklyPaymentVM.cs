using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Data.Models
{
    public class WeeklyPaymentVM
    {
        public int LaborId { get; set; }
        public string? LaborName { get; set; }
        public string? LaborType { get; set; }
        public int DaysPresent { get; set; }
        public decimal DailyWage { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal AdvanceDeducted { get; set; }
        public decimal NetPay { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
