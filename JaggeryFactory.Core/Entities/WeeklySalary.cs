using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    // JaggeryAgro.Core.Entities
    public class WeeklySalary
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public Labor? Labor { get; set; }
        public  string? LaborName { get; set; }
        public string? LaborType { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public int DaysPresent { get; set; }
        public decimal DailyRate { get; set; }
        public decimal AdvanceDeducted { get; set; }
        public decimal NetSalary { get; set; }
        public string? PaymentMethod { get; set; }

        public string Status { get; set; } = "Pending";
     
    }

}
