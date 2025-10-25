using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class LaborDashboardVM
    {
        public string LaborName { get; set; } = string.Empty;

        // Data collections
        public List<Attendance> AttendanceRecords { get; set; } = new();
        public List<CaneAdvance> AdvanceRecords { get; set; } = new();
        public List<WeeklySalary> SalaryRecords { get; set; } = new();

        // Summary data
        public int TotalDaysWorked => AttendanceRecords.Count(a => a.IsPresent);
        public decimal TotalAdvance => AdvanceRecords.Sum(a => a.Amount);
        public decimal TotalSalary => SalaryRecords.Sum(s => s.NetSalary);
        public decimal Balance => TotalSalary - TotalAdvance;
    }

}
