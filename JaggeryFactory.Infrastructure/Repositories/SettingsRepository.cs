using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _context;
        private decimal? _cachedRate;

        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetValueAsync(string key)
        {
            var setting = await _context.Settings
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.KeyName == key);

            return setting?.Value;
        }

        public async Task<decimal> GetDailyRateAsync()
        {
            if (_cachedRate.HasValue) return _cachedRate.Value;

            var value = await _context.Settings
                .AsNoTracking()
                .Where(s => s.KeyName == "DailyRate")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();

            _cachedRate = value != null ? decimal.Parse(value) : 0;
            return _cachedRate.Value;
        }

        public async Task SetValueAsync(string key, string value)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.KeyName == key);

            if (setting != null)
            {
                setting.Value = value;
            }
            else
            {
                await _context.Settings.AddAsync(new Setting { KeyName = key, Value = value });
            }

            await _context.SaveChangesAsync();

            // Reset cache if daily rate updated
            if (key == "DailyRate")
            {
                _cachedRate = null;
            }
        }
    }
}
