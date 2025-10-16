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
        public DateTime PaymentDate { get; set; }
        public decimal GrossAmount { get; set; }   // Attendance × DailyRate
        public decimal AdvanceAdjusted { get; set; }
        public decimal NetAmount { get; set; }     // Gross - Advance
        public bool IsPaid { get; set; }           // Flag if slip already generated

        // ✅ Add these:
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        
        public int AttendanceDays { get; set; } // New property to store attendance days

        public int rate { get; set; } // New property to store daily rate

    }
}
