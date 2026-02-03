using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Data.Models
{
    public class WeeklyPaymentViewModel
    {
        public int LaborId { get; set; } // For binding
        public string? LaborName { get; set; }
        public string? LaborType { get; set; }
        public int DaysPresent { get; set; }
        public decimal DailyWage { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal AdvanceDeducted { get; set; }
        public decimal NetPay { get; set; }
    }

}
