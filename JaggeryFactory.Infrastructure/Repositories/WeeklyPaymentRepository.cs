using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class WeeklyPaymentRepository : IWeeklyPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISettingsRepository _settingsRepo;

        public WeeklyPaymentRepository(ApplicationDbContext context, ISettingsRepository settingsRepo)
        {
            _context = context;
            _settingsRepo = settingsRepo;
        }

        public async Task<IEnumerable<WeeklyPayment>> GetAllAsync()
        {
            return await _context.WeeklyPayments
                           .Include(w => w.Labor)
                           .ThenInclude(l => l.LaborType)
                           .ToListAsync();
        }

        public async Task<WeeklyPayment?> GetByIdAsync(int id)
        {
            return await _context.WeeklyPayments
                           .Include(w => w.Labor)
                           .ThenInclude(l => l.LaborType)
                           .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(WeeklyPayment payment)
        {
            await _context.WeeklyPayments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _context.WeeklyPayments.FindAsync(id);
            if (payment != null)
            {
                _context.WeeklyPayments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task GenerateWeeklyPaymentsAsync(DateTime weekStart, DateTime weekEnd)
        {
            var dailyRate = await _settingsRepo.GetDailyRateAsync();
            var labors = await _context.Labors.Include(l => l.LaborType).ToListAsync();

            foreach (var labor in labors)
            {
                var attendanceDays = await _context.Attendances
                    .Where(a => a.LaborId == labor.Id &&
                                a.Date >= weekStart &&
                                a.Date <= weekEnd &&
                                a.IsPresent)
                    .CountAsync();

                var totalAdvance = await _context.AdvancePayments
                    .Where(a => a.LaborId == labor.Id &&
                                a.DateGiven >= weekStart &&
                                a.DateGiven <= weekEnd)
                    .SumAsync(a => a.Amount);

                dailyRate = labor.LaborType?.DailyWage ?? 0;
                var salary = (attendanceDays * dailyRate) - totalAdvance;

                var weeklyPayment = new WeeklyPayment
                {
                    LaborId = labor.Id,
                    WeekStartDate = weekStart,
                    WeekEndDate = weekEnd,
                    DaysPresent = attendanceDays,
                    DailyRate = dailyRate,
                    AdvanceDeducted = totalAdvance,   // ✅ fix: was wrongly set to salary
                    NetSalary = salary,
                    PaymentDate = DateTime.Now
                };

                await _context.WeeklyPayments.AddAsync(weeklyPayment);
            }

            await _context.SaveChangesAsync();
        }
    }
}
