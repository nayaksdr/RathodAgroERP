using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Data.Models;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JaggeryAgro.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISettingsRepository _settingsRepo;
        private readonly ILaborRepository _laborRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IAdvancePaymentRepository _advancePaymentRepo;

        

        public PaymentRepository(ApplicationDbContext context, ISettingsRepository settingsRepo, ILaborRepository laborRepo, IAttendanceRepository attendanceRepo, IAdvancePaymentRepository advancePaymentRepo)
        {
            _context = context;
            _settingsRepo = settingsRepo;
            _laborRepo = laborRepo;
            _attendanceRepo = attendanceRepo;
            _advancePaymentRepo = advancePaymentRepo;
           
        }

        //public IEnumerable<WeeklyPayment> GetPayments()
        //{
        //    return _context.WeeklyPayments.ToList();
        //}
        // Implement the interface method exactly
        // 
        public async Task<IEnumerable<WeeklyPayment>> GetWeeklyPayments()        
        {
            var dailyRateMultiplier = await _settingsRepo.GetDailyRateAsync();

            DateTime startDate = DateTime.Today.AddDays(-7); // 7 days ago
            DateTime endDate = DateTime.Today;

            // ✅ Get all labors (no labor type filter)
            var labors = _laborRepo.GetAll();

            // ✅ Get attendance (no labor type filter)
            var attendanceRecords = await _attendanceRepo.GetAttendanceBetweenDatesAsync(startDate, endDate);


            var attendanceData = attendanceRecords
                .GroupBy(a => a.LaborId)
                .Select(g => new
                {
                    LaborId = g.Key,
                    PresentDays = g.Count(a => a.IsPresent)
                })
                .ToList();

            // ✅ Get advances (no labor type filter)
            // ✅ Get advances only
            var advanceRecords = await _advancePaymentRepo
                .GetAdvancesBetweenDatesAsync(startDate, endDate);

            var advanceData = advanceRecords
                .GroupBy(a => a.LaborId)
                .Select(g => new
                {
                    LaborId = g.Key,
                    TotalAdvance = g.Sum(x => x.Amount)
                })
                .ToList();

            // ✅ Build payments for all labors
            return labors.Select(labor =>
            {
                var att = attendanceData.FirstOrDefault(a => a.LaborId == labor.Id);
                var adv = advanceData.FirstOrDefault(a => a.LaborId == labor.Id);
                var rate = labor.LaborType?.DailyWage ?? 0;

                return new WeeklyPayment
                {
                    LaborId = labor.Id,
                    LaborName = labor.Name!,
                    DailyRate = rate,
                    DaysPresent = att?.PresentDays ?? 0,
                    AdvanceDeducted = adv?.TotalAdvance ?? 0,
                    NetSalary = ((att?.PresentDays ?? 0) * rate * dailyRateMultiplier)
                                - (adv?.TotalAdvance ?? 0)
                };
            }).ToList();
        }

        public async Task<bool> IsPaymentAlreadyDoneAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.WeeklyPayments
                .AnyAsync(p => p.WeekStartDate == fromDate && p.WeekEndDate == toDate);
        }

        public async Task SaveWeeklyPaymentsAsync(List<WeeklyPayment> payments)
        {
            // Get global daily rate from settings
            var dailyRate = await _settingsRepo.GetDailyRateAsync();

            // Map payments to WeeklyPayment entities using global dailyRate
            var entities = payments.Select(p => new WeeklyPayment
            {
                LaborId = p.LaborId,
                WeekStartDate = p.WeekStart,
                WeekEndDate = p.WeekEnd,
                DaysPresent = p.DaysPresent,

                // Use the global daily rate
                DailyRate = dailyRate,

                AdvanceDeducted = p.AdvanceDeducted,

                // Recalculate NetSalary using global dailyRate
                NetSalary = (p.DaysPresent * p.DailyRate * dailyRate) - p.AdvanceDeducted,

                PaymentDate = DateTime.Now
            }).ToList();

            await _context.WeeklyPayments.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }


        public IEnumerable<WeeklyPayment> GetPaymentsAll(int laborId, DateTime fromDate, DateTime toDate)
        {
            return _context.WeeklyPayments
                .Where(p => p.LaborId == laborId &&
                            p.WeekStart == fromDate &&
                            p.WeekEnd == toDate)
                .ToList();
        }

        public IEnumerable<Payment> GetAll() => _context.Payments.ToList();
        public void AddPayment(WeeklyPayment payment)
        {
            _context.WeeklyPayments.Add(payment); // or your DbSet name
            Save();
            //_context.SaveChanges();
            //_context.SaveChanges();
        }

        public Payment GetById(int id)
        {
            var payment = _context.Payments.Find(id);
            if (payment == null)
                throw new InvalidOperationException($"Payment with Id {id} not found.");
            return payment;
        }


        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
            Save();
        }

        public void Update(Payment payment)
        {
            _context.Payments.Update(payment);
            Save();
        }

        public void Delete(int id)
        {
            var pay = GetById(id);
            if (pay != null)
            {
                _context.Payments.Remove(pay);
                Save();
            }
        }
       
        public void Save() => _context.SaveChangesAsync();

        public async Task<List<WeeklyPayment>> GetWeeklyPaymentsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Payments.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(p => p.WeekStart >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(p => p.WeekEnd <= toDate.Value);

            // Project Payment to WeeklyPayment
            var weeklyPayments = await query
                .Select(p => new WeeklyPayment
                {
                    LaborId = p.LaborId,
                    LaborName = p.Labor.Name,
                
                   
                    PaymentDate = p.PaidOn
                })
                .ToListAsync();

            return weeklyPayments;
        }

        IEnumerable<WeeklyPayment> IPaymentRepository.GetWeeklyPayments()
        {
            throw new NotImplementedException();
        }
    }
}
