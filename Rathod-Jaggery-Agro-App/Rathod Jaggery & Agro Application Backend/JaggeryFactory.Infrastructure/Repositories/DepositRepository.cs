using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class DepositRepository : IDepositRepository
    {
        private readonly ApplicationDbContext _context;

        public DepositRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Deposit>> GetAllAsync()
        {
            return await _context.Deposits.ToListAsync();
        }

        public async Task<Deposit?> GetByIdAsync(int id)
        {
            return await _context.Deposits.FindAsync(id);
        }

        public async Task<Deposit> AddAsync(Deposit deposit)
        {
            await _context.Deposits.AddAsync(deposit);
            await _context.SaveChangesAsync();
            return deposit;
        }

        public async Task UpdateAsync(Deposit deposit)
        {
            _context.Deposits.Update(deposit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Deposits.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
