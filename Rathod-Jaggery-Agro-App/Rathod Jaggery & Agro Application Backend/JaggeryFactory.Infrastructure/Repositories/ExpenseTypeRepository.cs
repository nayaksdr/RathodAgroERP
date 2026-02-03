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
    public class ExpenseTypeRepository : IExpenseTypeRepository
    {

        private readonly ApplicationDbContext _context;

        public ExpenseTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseType>> GetAllAsync()
        {
            return await _context.ExpenseTypes.ToListAsync();
        }

        public async Task<ExpenseType> GetByIdAsync(int id)
        {
            return await _context.ExpenseTypes.FindAsync(id);
        }

        public async Task AddAsync(ExpenseType expenseType)
        {
            _context.ExpenseTypes.Add(expenseType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExpenseType expenseType)
        {
            _context.ExpenseTypes.Update(expenseType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ExpenseTypes.FindAsync(id);
            if (entity != null)
            {
                _context.ExpenseTypes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
