
using JaggeryAgro.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Services
{
    public interface IWeeklySalaryService
    {
        /// <summary>
        /// दिलेल्या तारखांच्या रेंजमध्ये साप्ताहिक वेतन रिपोर्ट मिळवा
        /// </summary>
        /// <param name="fromDate">सुरुवातीची तारीख</param>
        /// <param name="toDate">शेवटची तारीख</param>
        /// <returns>Weekly Salary Report List</returns>
        Task<List<WeeklySalaryReportVM>> GetWeeklyReportAsync(DateTime fromDate, DateTime toDate);
    }
}
