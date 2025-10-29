using DocumentFormat.OpenXml.InkML;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class CanePurchaseRepository : GenericRepository<CanePurchase>, ICanePurchaseRepository
    {
        private readonly ApplicationDbContext _db;
        public CanePurchaseRepository(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(CanePurchase purchase)
        {
            _db.CanePurchases.Add(purchase);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.CanePurchases.FindAsync(id);
            if (ent != null) { _db.CanePurchases.Remove(ent); await _db.SaveChangesAsync(); }
        }

        public async Task<IEnumerable<CanePurchase>> GetAllAsync(DateTime? from = null, DateTime? to = null, int? farmerId = null)
        {
            var q = _db.CanePurchases.Include(p => p.Farmer).AsQueryable();
            if (from.HasValue) q = q.Where(p => p.PurchaseDate >= from.Value.Date);
            if (to.HasValue) q = q.Where(p => p.PurchaseDate <= to.Value.Date.AddDays(1).AddTicks(-1));
            if (farmerId.HasValue) q = q.Where(p => p.FarmerId == farmerId.Value);
            return await q.OrderByDescending(p => p.PurchaseDate).ToListAsync();
        }      

        public async Task<CanePurchase> GetByIdAsync(int id) =>
            await _db.CanePurchases.Include(p => p.Farmer).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<CanePurchase>> GetByFarmerAsync(int farmerId) =>
            await _db.CanePurchases.Where(p => p.FarmerId == farmerId).OrderByDescending(p => p.PurchaseDate).ToListAsync();
        public async Task<IEnumerable<CanePurchase>> GetByFarmerAsyncNew(int farmerId)
        {
            return await _db.CanePurchases
                .Include(p => p.Farmer)
                .Include(p => p.Payments)
                .Where(p => p.FarmerId == farmerId)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();
        }
        public async Task UpdateAsync(CanePurchase purchase)
        {
            _db.CanePurchases.Update(purchase);
            await _db.SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public Task<IEnumerable<CanePurchase>> GetByFarmerBetweenAsync(int farmerId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
        public async Task<decimal> GetTotalTonsByLaborInRangeAsync(int laborId, DateTime from, DateTime to)
        {
            return await _db.CanePurchases
                .Where(cp => cp.LaborId == laborId && cp.PurchaseDate >= from && cp.PurchaseDate <= to)
                .SumAsync(cp => (decimal?)cp.QuantityTon ?? 0m);
        }


    }
}

