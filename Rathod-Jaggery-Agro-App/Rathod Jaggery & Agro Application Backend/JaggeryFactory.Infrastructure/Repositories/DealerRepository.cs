using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class DealerRepository : IDealerRepository
    {
        private readonly ApplicationDbContext _ctx;

        public DealerRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IEnumerable<Dealer>> GetAllAsync(string search, int page, int pageSize)
        {
            var query = _ctx.Dealers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.Name.Contains(search) || d.Name.Contains(search));
            }

            return await query
                .OrderByDescending(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Dealer>> GetDealerAsync()
        {
            return await _ctx.Dealers
                .OrderBy(x => x.Name)
                .ToListAsync();
        }        

        public async Task<int> GetCountAsync(string search)
        {
            var query = _ctx.Dealers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.Name.Contains(search) || d.ContactNumber.Contains(search));
            }

            return await query.CountAsync();
        }

        //public async Task<List<Dealer>> GetAllAsync()
        //{
        //    return await _ctx.Dealers
        //        .OrderBy(d => d.Name)
        //        .ToListAsync();
        //}

        public async Task<Dealer> GetByIdAsync(int id)
        {
            return await _ctx.Dealers.FindAsync(id);
        }

        public async Task AddAsync(Dealer dealer)
        {
            await _ctx.Dealers.AddAsync(dealer);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dealer dealer)
        {
            _ctx.Dealers.Update(dealer);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dealer = await GetByIdAsync(id);
            if (dealer != null)
            {
                _ctx.Dealers.Remove(dealer);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Dealer>> GetAllAsync()
        {
            return await _ctx.Dealers
                                 .OrderBy(d => d.Name)
                                 .ToListAsync();
        }

        
    }
}
