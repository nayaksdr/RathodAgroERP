using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class CaneAdvanceRepository : ICaneAdvanceRepository
    {
        private readonly ApplicationDbContext _db;

        public CaneAdvanceRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<decimal> GetAdvanceByFarmerAsync(int farmerId)
        {
            return await _db.CaneAdvances
                .Where(a => a.FarmerId == farmerId)
                .SumAsync(a => a.AdvanceAmount);
        }

        public async Task<CaneAdvance> AddAsync(CaneAdvance entity)
        {
            entity.RemainingAmount = entity.Amount;
            await _db.CaneAdvances.AddAsync(entity);  // 👈 changed to async
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(CaneAdvance entity)
        {
            _db.CaneAdvances.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<CaneAdvance?> GetAsync(int id)
        {
            return await _db.CaneAdvances
                .Include(a => a.Farmer)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<CaneAdvance>> GetByFarmerAsync(int farmerId)
        {
            return await _db.CaneAdvances
                .Where(x => x.FarmerId == farmerId)
                .OrderBy(x => x.AdvanceDate)
                .ToListAsync();
        }

        public async Task<List<CaneAdvance>> GetAdvancePayByFarmerAsync(int farmerId)
        {
            return await _db.CaneAdvances
                .Where(x => x.FarmerId == farmerId)
                .OrderBy(x => x.AdvanceDate)
                .Select(x => new CaneAdvance
                {
                    AdvanceDate = x.AdvanceDate,
                    Amount = x.Amount
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CaneAdvance>> GetOpenByFarmerAsync(int farmerId)
        {
            return await _db.CaneAdvances
                .Where(x => x.FarmerId == farmerId && x.RemainingAmount > 0)
                .OrderBy(x => x.AdvanceDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CaneAdvance>> GetAllAsync()
        {
            return await _db.CaneAdvances
                .Include(p => p.Farmer)   // include Farmer details if needed
                .ToListAsync();
        }

        public async Task<Dictionary<int, decimal>> GetAllAdvancesAsync()
        {
            return await _db.CaneAdvances
                .GroupBy(a => a.FarmerId)
                .Select(g => new { FarmerId = g.Key, TotalAdvance = g.Sum(x => x.AdvanceAmount) })
                .ToDictionaryAsync(x => x.FarmerId, x => x.TotalAdvance);
        }

        public async Task<CaneAdvance?> GetByIdAsync(int id)
        {
            return await _db.CaneAdvances
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task DeleteAsync(CaneAdvance advance)
        {
            _db.CaneAdvances.Remove(advance);
            await _db.SaveChangesAsync();
        }
    }
}
