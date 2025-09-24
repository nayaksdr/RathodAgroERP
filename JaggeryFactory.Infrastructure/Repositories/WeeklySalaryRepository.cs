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
    public class WeeklySalaryRepository : IWeeklySalaryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISettingsRepository _settingsRepo;

        public WeeklySalaryRepository(ApplicationDbContext context, ISettingsRepository settingsRepo)
        {
            _context = context;
            _settingsRepo = settingsRepo;
        }

        public async Task<IEnumerable<WeeklySalary>> GetAllAsync()
        {
            return await _context.WeeklySalaries
                                 .Include(w => w.Labor)
                                 .ToListAsync();
        }
        public async Task AddAsync(WeeklySalary salary)
        {
            _context.WeeklySalaries.Add(salary);
            await _context.SaveChangesAsync();
        }

        

        public async Task UpdateStatusAsync(int id, string status)
        {
            var salary = await _context.WeeklySalaries.FindAsync(id);
            if (salary != null)
            {
                salary.Status = status;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<WeeklySalary?> GetByIdAsync(int id)
        {
            return await _context.WeeklySalaries
                                 .Include(w => w.Labor)
                                 .FirstOrDefaultAsync(w => w.Id == id);
        }       

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.WeeklySalaries.FindAsync(id);
            if (entity != null)
            {
                _context.WeeklySalaries.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task GenerateWeeklySalariesAsync(DateTime weekStart, DateTime weekEnd)
        {
            var labors = await _context.Labors
                                       .Include(l => l.LaborType)
                                       .ToListAsync();

            foreach (var labor in labors)
            {
                var presentDays = await _context.Attendances
                    .Where(a => a.LaborId == labor.Id &&
                                a.Date >= weekStart &&
                                a.Date <= weekEnd &&
                                a.IsPresent)
                    .CountAsync();

                var advance = await _context.AdvancePayments
                    .Where(a => a.LaborId == labor.Id &&
                                a.DateGiven >= weekStart &&
                                a.DateGiven <= weekEnd)
                    .SumAsync(a => a.Amount);

                var dailyRate = labor.LaborType?.DailyWage ?? await _settingsRepo.GetDailyRateAsync();

                var gross = presentDays * dailyRate;
                var net = gross - advance;

                var salary = new WeeklySalary
                {
                    LaborId = labor.Id,
                    WeekStart = weekStart,
                    WeekEnd = weekEnd,
                    DaysPresent = presentDays,
                    DailyRate = dailyRate,
                    AdvanceDeducted = advance,
                    NetSalary = net
                };

                await _context.WeeklySalaries.AddAsync(salary);
            }

            await _context.SaveChangesAsync();
        }
    }
}
