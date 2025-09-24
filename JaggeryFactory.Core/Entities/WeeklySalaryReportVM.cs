using System;

namespace JaggeryAgro.Core.ViewModels
{
    public class WeeklySalaryReportVM
    {
        public int LaborId { get; set; }
        public string? LaborName { get; set; }
        public string? LaborTypeName { get; set; }
        public int DaysPresent { get; set; }
        public decimal DailyRate { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal AdvanceDeducted { get; set; }
        public decimal NetSalary { get; set; }
        public string Status { get; set; } = "Pending";

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
