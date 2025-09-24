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
    public class ProduceRepository : IProduceRepository
    {
        private readonly ApplicationDbContext _db;
        public ProduceRepository(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(JaggeryProduce produce)
        {
            _db.JaggeryProduces.Add(produce);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _db.JaggeryProduces.FindAsync(id);
            if (ent == null) return;
            _db.JaggeryProduces.Remove(ent);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<JaggeryProduce>> GetAllAsync(DateTime? from = null, DateTime? to = null)
        {
            var q = _db.JaggeryProduces.AsQueryable();
            if (from.HasValue) q = q.Where(x => x.ProducedDate >= from.Value.Date);
            if (to.HasValue) q = q.Where(x => x.ProducedDate <= to.Value.Date.AddDays(1).AddTicks(-1));
            return await q.OrderByDescending(x => x.ProducedDate).ToListAsync();
        }

        public async Task<JaggeryProduce> GetByIdAsync(int id)
        {
            return await _db.JaggeryProduces.FindAsync(id);
        }

        public async Task UpdateAsync(JaggeryProduce produce)
        {
            _db.JaggeryProduces.Update(produce);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> BatchNumberExistsAsync(string batchNumber, int? exceptId = null)
        {
            var q = _db.JaggeryProduces.Where(x => x.BatchNumber == batchNumber);
            if (exceptId.HasValue) q = q.Where(x => x.Id != exceptId.Value);
            return await q.AnyAsync();
        }
        public async Task<decimal> GetTotalUnitPriceAsync()
        {
            return await _db.JaggeryProduces.SumAsync(x => x.UnitPrice);
        }

        public async Task<decimal> GetTotalCostAsync()
        {
            return await _db.JaggeryProduces.SumAsync(x => x.TotalCost);
        }
    }
}

