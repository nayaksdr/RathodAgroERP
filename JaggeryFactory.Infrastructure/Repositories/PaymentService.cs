using iTextSharp.text;
using iTextSharp.text.pdf;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class PaymentService
    {
        private readonly ILaborRepository _laborRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IAdvancePaymentRepository _advancePaymentRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly IPaymentRepository _paymentRepo;

        public PaymentService(ILaborRepository laborRepo, IAttendanceRepository attendanceRepo, IAdvancePaymentRepository advancePaymentRepo, ISettingsRepository settingsRepo, IPaymentRepository paymentRepo)
        {
            _laborRepo = laborRepo;
            _attendanceRepo = attendanceRepo;
            _advancePaymentRepo = advancePaymentRepo;
            _settingsRepo = settingsRepo;
            _paymentRepo = paymentRepo;
           
        }
        public async Task<List<WeeklyPayment>> GeneratePaymentsAsync(DateTime fromDate, DateTime toDate, string paymentMethod)
        {
            var dailyRate = await _settingsRepo.GetDailyRateAsync();
            var attendances = await _attendanceRepo.GetAttendanceBetweenDatesAsync(fromDate, toDate);
            var laborList = _laborRepo.GetAll();
            var result = new List<WeeklyPayment>();

            foreach (var labor in laborList)
            {
                // build WeeklyPayment object and add to result
                var payment = new WeeklyPayment
                {
                    LaborId = labor.Id,
                    LaborName = labor.Name!,
                    LaborType = labor.LaborType?.LaborTypeName ?? "N/A",
                    DaysPresent = attendances.Count(a => a.LaborId == labor.Id && a.IsPresent),
                    DailyRate = labor.LaborType?.DailyWage ?? 0,
                    AdvanceDeducted = (await _advancePaymentRepo
                        .GetAdvancesBetweenDatesAsync(fromDate, toDate))
                        .Sum(a => a.Amount),
                    NetSalary = ((attendances.Count(a => a.LaborId == labor.Id && a.IsPresent)) * (labor.LaborType?.DailyWage ?? 0)) - (await _advancePaymentRepo
                        .GetAdvancesBetweenDatesAsync(fromDate, toDate))
                        .Sum(a => a.Amount),
                    PaymentMethod = paymentMethod,
                    PaymentDate = DateTime.Now,
                    WeekStartDate = fromDate,
                    WeekEndDate = toDate,
                    IsPaid = true
                };

                result.Add(payment);
            }

            return result; // ✅ Always return here
        }
        //public List<WeeklyPayment> GenerateWeeklyPayments(DateTime fromDate, DateTime toDate, string paymentMethod)
        //{
        //    var dailyRate = _settingsRepo.GetDailyRate(); 
        //    var attendances = _attendanceRepo.GetAttendanceBetweenDates(fromDate, toDate);
        //    var laborList = _laborRepo.GetAll();
        //    var result = new List<WeeklyPayment>();

        //    foreach (var labor in laborList)
        //    {
        //        // build WeeklyPayment object and add to result
        //        var payment = new WeeklyPayment
        //        {
        //            LaborId = labor.Id,
        //            LaborName = labor.Name,
        //            LaborType = labor.LaborType?.LaborTypeName ?? "N/A",
        //            DaysPresent = attendances.Count(a => a.LaborId == labor.Id && a.IsPresent),
        //            DailyRate = labor.LaborType?.DailyWage ?? 0,
        //            AdvanceDeducted = (await _advancePaymentRepo
        //                .GetAdvancesBetweenDatesAsync(labor.Id, fromDate, toDate))
        //                .Sum(a => a.Amount),
        //            NetSalary = ((attendances.Count(a => a.LaborId == labor.Id && a.IsPresent)) *
        //     (labor.LaborType?.DailyWage ?? 0))
        //     - (await _advancePaymentRepo
        //                .GetAdvancesBetweenDatesAsync(labor.Id, fromDate, toDate))
        //                .Sum(a => a.Amount),
        //            PaymentMethod = paymentMethod,
        //            PaymentDate = DateTime.Now,
        //            WeekStartDate = fromDate,
        //            WeekEndDate = toDate,
        //            IsPaid = true
        //        };

        //        result.Add(payment);
        //    }

        //    return result; // ✅ Always return here
        //}

       

        public bool HasPaymentRecord(int laborId, DateTime startDate, DateTime endDate)
        {
            return _paymentRepo
                .GetAll()
                .Any(p => p.LaborId == laborId && p.WeekStart == startDate && p.WeekEnd == endDate && p.IsPaid);
        }

      

        public void SavePayment(WeeklyPayment payment)
        {
            _paymentRepo.AddPayment(payment);
            _paymentRepo.Save();
        }        

        public async Task GenerateWeeklyPaymentsAsync(DateTime weekStart)
        {
            var dailyRate = await _settingsRepo.GetDailyRateAsync();

            DateTime weekEnd = weekStart.AddDays(6);

            var labors = _laborRepo.GetAllWithLaborType(); // Includes DailyWage

            foreach (var labor in labors)
            {
                // ✅ 1. Get attendance for the week
                var attendance = await _attendanceRepo.GetAttendanceBetweenDatesAsync(weekStart, weekEnd);

                int presentDays = attendance.Count(a => a.IsPresent);
                decimal dailyWage = labor.LaborType?.DailyWage ?? 0; // fixed from .Id
                 dailyRate = await _settingsRepo.GetDailyRateAsync();
                decimal grossSalary = presentDays * dailyRate;

                // ✅ 2. Get advance payments in this week (await works now)
                var advancePayments = await _advancePaymentRepo
                    .GetAdvancesBetweenDatesAsync(weekStart, weekEnd);

                decimal totalAdvance = advancePayments.Sum(a => a.Amount);

                // ✅ 3. Final salary
                decimal netSalary = grossSalary - totalAdvance;
                if (netSalary < 0) netSalary = 0;

                // ✅ 4. Save Payment
                var payment = new Payment
                {
                    LaborId = labor.Id,
                    WeekStart = weekStart,
                    WeekEnd = weekEnd,
                    Amount = netSalary,
                    GeneratedDate = DateTime.Now
                };

                _paymentRepo.Add(payment);
            }
        }

    }

}
