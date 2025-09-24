using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class DealerAdvanceRepository : IDealerAdvanceRepository
    {
        private readonly ApplicationDbContext _ctx;

        public DealerAdvanceRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }


        public async Task<decimal> GetDealerAdvanceAsync(int dealerId)
        {
            return await _ctx.DealerAdvances
                .Where(a => a.DealerId == dealerId)
                .SumAsync(a => (decimal?)a.Amount) ?? 0;   // safe null check
        }
        public async Task AddAsync(DealerAdvance entity)
        {
            _ctx.DealerAdvances.Add(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _ctx.DealerAdvances.FindAsync(id);
            if (item != null)
            {
                _ctx.DealerAdvances.Remove(item);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DealerAdvance>> GetAllAsync()
        {
            return await _ctx.DealerAdvances
                .Include(x => x.Dealer)
                .OrderByDescending(x => x.AdvanceDate)
                .ToListAsync();
        }

        public async Task<DealerAdvance> GetByIdAsync(int id)
        {
            return await _ctx.DealerAdvances
                .Include(x => x.Dealer)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(DealerAdvance entity)
        {
            _ctx.DealerAdvances.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<DealerAdvance>> FilterAsync(int? dealerId, DateTime? from, DateTime? to)
        {
            var q = _ctx.DealerAdvances.Include(x => x.Dealer).AsQueryable();

            if (dealerId.HasValue && dealerId.Value > 0)
                q = q.Where(x => x.DealerId == dealerId.Value);

            if (from.HasValue)
                q = q.Where(x => x.AdvanceDate.Date >= from.Value.Date);

            if (to.HasValue)
                q = q.Where(x => x.AdvanceDate.Date <= to.Value.Date);

            return await q.OrderByDescending(x => x.AdvanceDate).ToListAsync();
        }

        public async Task<decimal> GetTotalAdvanceByDealerAsync(int dealerId, DateTime? uptoDate = null)
        {
            var q = _ctx.DealerAdvances.Where(x => x.DealerId == dealerId);
            if (uptoDate.HasValue)
                q = q.Where(x => x.AdvanceDate.Date <= uptoDate.Value.Date);

            return await q.SumAsync(x => (decimal?)x.Amount) ?? 0m;
        }
        public async Task<decimal> GetTotalAdvanceByDealer(int dealerId)
        {
            return await _ctx.DealerAdvances
                .Where(a => a.DealerId == dealerId)
                .SumAsync(a => (decimal?)a.Amount) ?? 0;   // handles null
        }
    }
}
