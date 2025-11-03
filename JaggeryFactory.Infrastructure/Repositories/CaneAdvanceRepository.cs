using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class CaneAdvanceRepository : ICaneAdvanceRepository
    {
        private readonly ApplicationDbContext _db;

        public CaneAdvanceRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // 🔹 Get total advance amount for a farmer
        public async Task<decimal> GetAdvanceByFarmerAsync(int farmerId)
        {
            return await _db.CaneAdvances
                .Where(a => a.FarmerId == farmerId)
                .SumAsync(a => (decimal?)a.Amount ?? 0m); // ✅ Handles nulls safely
        }

        // 🔹 Add new advance
        public async Task<CaneAdvance> AddAsync(CaneAdvance entity)
        {
            entity.RemainingAmount = entity.Amount; // ✅ Keep consistency
            await _db.CaneAdvances.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        // 🔹 Update existing advance
        public async Task UpdateAsync(CaneAdvance entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        // 🔹 Get single advance by ID (includes Farmer & Member)
        public async Task<CaneAdvance?> GetAsync(int id)
        {
            return await _db.CaneAdvances
                .Include(a => a.Farmer)
                .Include(a => a.Member)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // 🔹 Get advances by Farmer (detailed list)
        public async Task<IEnumerable<CaneAdvance>> GetByFarmerAsync(int farmerId)
        {
            return await _db.CaneAdvances
                .Include(a => a.Farmer)
                .Include(a => a.Member)
                .Where(a => a.FarmerId == farmerId)
                .OrderBy(a => a.AdvanceDate)
                .ToListAsync();
        }

        // 🔹 Get simplified advance payments (for payment summary)
        public async Task<List<CaneAdvance>> GetAdvancePayByFarmerAsync(int farmerId)
        {
            return await _db.CaneAdvances
                .Where(a => a.FarmerId == farmerId)
                .OrderBy(a => a.AdvanceDate)
                .Select(a => new CaneAdvance
                {
                    AdvanceDate = a.AdvanceDate,
                    Amount = a.Amount
                })
                .ToListAsync();
        }

        // 🔹 Get open advances (remaining amount > 0)
        public async Task<IEnumerable<CaneAdvance>> GetOpenByFarmerAsync(int farmerId)
        {
            return await _db.CaneAdvances
                .Include(a => a.Farmer)
                .Include(a => a.Member)
                .Where(a => a.FarmerId == farmerId && a.RemainingAmount > 0)
                .OrderBy(a => a.AdvanceDate)
                .ToListAsync();
        }

        // 🔹 Get all advances (for index page)
        public async Task<IEnumerable<CaneAdvance>> GetAllAsync()
        {
            return await _db.CaneAdvances
                .Include(a => a.Farmer)
                .Include(a => a.Member)
                .OrderByDescending(a => a.AdvanceDate)
                .ToListAsync();
        }

        // 🔹 Get total advances grouped by Farmer
        public async Task<Dictionary<int, decimal>> GetAllAdvancesAsync()
        {
            return await _db.CaneAdvances
                .GroupBy(a => a.FarmerId)
                .Select(g => new
                {
                    FarmerId = g.Key,
                    TotalAdvance = g.Sum(a => a.Amount)
                })
                .ToDictionaryAsync(g => g.FarmerId, g => g.TotalAdvance);
        }

        // 🔹 Get single advance by ID (used for Edit/Delete)
        public async Task<CaneAdvance?> GetByIdAsync(int id)
        {
            return await _db.CaneAdvances
                .Include(a => a.Farmer)
                .Include(a => a.Member)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // 🔹 Delete advance
        public async Task DeleteAsync(CaneAdvance advance)
        {
            _db.CaneAdvances.Remove(advance);
            await _db.SaveChangesAsync();
        }
    }
}
