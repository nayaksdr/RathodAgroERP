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
    public class CaneProcessingRepository : ICaneProcessingRepository
    {
        private readonly ApplicationDbContext _context;

        public CaneProcessingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        

        // 🔹 Fetch all processing entries within a date range (with labor details)
        public async Task<IEnumerable<CaneProcessing>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.CaneProcessings
                .AsNoTracking()
                .Include(x => x.Labor)
                .Where(x => x.Date >= from && x.Date <= to)
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        // 🔹 Add a new cane processing record
        public async Task AddAsync(CaneProcessing entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.CaneProcessings.AddAsync(entity);
        }

        // 🔹 Save all pending changes
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<decimal> GetTotalTonsByLaborInRangeAsync(int laborId, DateTime from, DateTime to)
        {
            return await _context.CanePurchases
                .Where(x => x.LaborId == laborId && x.PurchaseDate >= from && x.PurchaseDate <= to)
                .SumAsync(x => (decimal?)x.QuantityTon) ?? 0m;
        }

    }
}
