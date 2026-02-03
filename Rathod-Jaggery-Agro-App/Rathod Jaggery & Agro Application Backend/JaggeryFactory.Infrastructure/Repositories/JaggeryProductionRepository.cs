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
    public class JaggeryProductionRepository : IJaggeryProductionRepository
    {
        private readonly ApplicationDbContext _context;

        public JaggeryProductionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Get total jaggery production (in quintals) by a specific labor within a date range
        public async Task<decimal> GetTotalProductionByLaborInRangeAsync(int laborId, DateTime from, DateTime to)
        {
            if (laborId <= 0)
                throw new ArgumentException("Invalid LaborId provided.", nameof(laborId));

            // Using nullable sum to safely handle empty results
            var totalProduction = await _context.JaggeryProductions
                .AsNoTracking()
                .Where(x => x.LaborId == laborId && x.Date >= from && x.Date <= to)
                .SumAsync(x => (decimal?)x.Quantity) ?? 0m;

            return totalProduction;
        }

        // 🔹 Fetch all jaggery production entries within a given date range (with labor details)
        public async Task<IEnumerable<JaggeryProduction>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.JaggeryProductions
                .AsNoTracking()
                .Include(x => x.Labor)
                .Where(x => x.Date >= from && x.Date <= to)
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        // 🔹 Add a new jaggery production record
        public async Task AddAsync(JaggeryProduction entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.JaggeryProductions.AddAsync(entity);
        }

        // 🔹 Save all pending changes to the database
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
