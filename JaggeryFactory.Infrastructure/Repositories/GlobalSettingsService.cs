using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class GlobalSettingsService : IGlobalSettingsService
    {
        private readonly ApplicationDbContext _context;

        public GlobalSettingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Async version of GetDailyRate
        public async Task<decimal> GetDailyRateAsync()
        {
            var rateValue = await _context.Settings
                .AsNoTracking()
                .Where(s => s.KeyName == "DailyRate")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();

            return decimal.TryParse(rateValue, out var result) ? result : 0;
        }

        // Async version of UpdateDailyRate
        public async Task UpdateDailyRateAsync(decimal rate)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.KeyName == "DailyRate");

            if (setting != null)
            {
                setting.Value = rate.ToString();
            }
            else
            {
                // If the DailyRate row doesn't exist yet, create it
                setting = new Setting
                {
                    KeyName = "DailyRate",
                    Value = rate.ToString()
                };
                await _context.Settings.AddAsync(setting);
            }

            await _context.SaveChangesAsync();
        }
    }
}
