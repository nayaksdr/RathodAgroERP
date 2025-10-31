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
    public class LaborTypeRateRepository : ILaborTypeRateRepository
    {
        private readonly ApplicationDbContext _context;

        public LaborTypeRateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LaborTypeRate>> GetAllRatesAsync()
        {
            return await _context.LaborTypeRates
                .Include(r => r.LaborType)
                .AsNoTracking()
                .ToListAsync();

        }           

        
        // In LaborTypeRateRepository
        public async Task<LaborTypeRate> GetCurrentRateByLaborTypeIdAsync(int laborTypeId)
        {
            return await _context.LaborTypeRates
                .Where(r => r.LaborTypeId == laborTypeId && r.EffectiveFrom <= DateTime.Now)
                .OrderByDescending(r => r.EffectiveFrom)
                .FirstOrDefaultAsync();
        }       


        public async Task AddRateAsync(LaborTypeRate rate)
        {
            if (rate == null)
                throw new ArgumentNullException(nameof(rate));

            _context.LaborTypeRates.Add(rate);
            await _context.SaveChangesAsync();
        }

        // ------------------- NEW METHODS -------------------

        public async Task UpdateRateAsync(LaborTypeRate rate)
        {
            if (rate == null)
                throw new ArgumentNullException(nameof(rate));

            _context.LaborTypeRates.Update(rate);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteRateAsync(int id)
        {
            var existing = await _context.LaborTypeRates.FindAsync(id);
            if (existing != null)
            {
                _context.LaborTypeRates.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
       
        public async Task<LaborTypeRate> GetByIdAsync(int id)
        {
            return await _context.LaborTypeRates
                .Include(r => r.LaborType)
                .FirstOrDefaultAsync(r => r.Id == id);
        }        

        public async Task<decimal> GetPerTonRateAsync(int laborTypeId)
        {
            var rate = await GetCurrentRateByLaborTypeIdAsync(laborTypeId);
            return rate?.PerTonRate ?? 0m;
        }

        public async Task<decimal> GetPerProductionRateAsync(int laborTypeId)
        {
            var rate = await GetCurrentRateByLaborTypeIdAsync(laborTypeId);
            return rate?.PerProductionRate ?? 0m;
        }
    }
}
