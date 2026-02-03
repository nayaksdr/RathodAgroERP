using JaggeryAgro.Core.Entities;

using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Services
{
    public class WeeklySalaryService : IWeeklySalaryService
    {
        private readonly ILaborRepository _laborRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IAdvancePaymentRepository _advanceRepo;
        private readonly ISettingsRepository _settingsRepo;

        public WeeklySalaryService(
            ILaborRepository laborRepo,
            IAttendanceRepository attendanceRepo,
            IAdvancePaymentRepository advanceRepo,
            ISettingsRepository settingsRepo)
        {
            _laborRepo = laborRepo;
            _attendanceRepo = attendanceRepo;
            _advanceRepo = advanceRepo;
            _settingsRepo = settingsRepo;
        }

        public async Task<List<WeeklySalaryReportVM>> GetWeeklyReportAsync(DateTime fromDate, DateTime toDate)
        {
            var labors = await _laborRepo.GetAllAsync();
            var attendances = await _attendanceRepo.GetAttendanceBetweenDatesAsync(fromDate, toDate);

            var report = new List<WeeklySalaryReportVM>();

            foreach (var labor in labors)
            {
                // उपस्थित दिवस
                var daysPresent = attendances.Count(a => a.LaborId == labor.Id);

                // दैनंदिन वेतन (LaborType मधून किंवा Default)
                var dailyRate = labor.LaborType?.DailyWage > 0
                    ? labor.LaborType.DailyWage
                    : await _settingsRepo.GetDailyRateAsync();

                var grossPay = daysPresent * dailyRate;

                // Advance मिळवणे
                var advance = await _advanceRepo.GetAdvanceByLaborInRangeAsync(labor.Id, fromDate, toDate);

                var netPay = grossPay - advance;

                report.Add(new WeeklySalaryReportVM
                {
                    LaborId = labor.Id,
                    LaborName = labor.Name,
                    DaysPresent = daysPresent,
                    DailyRate = dailyRate,
                    GrossSalary = grossPay,
                    AdvanceDeducted = advance,
                    NetSalary = netPay,
                    Status = "Pending"
                });
            }

            return report;
        }
    }
}
