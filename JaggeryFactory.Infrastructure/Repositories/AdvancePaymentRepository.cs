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
    public class AdvancePaymentRepository : IAdvancePaymentRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public AdvancePaymentRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<List<AdvancePayment>> GetAllAsync()
        {
            await using var context = _contextFactory.CreateDbContext();

            return await context.AdvancePayments
                .Include(a => a.Labor)
                .OrderByDescending(a => a.DateGiven)
                .ToListAsync();
        }

        public async Task<IEnumerable<AdvancePayment>> GetAdvanceByLaborAsync(int laborId)
        {
            await using var context = _contextFactory.CreateDbContext();

            return await context.AdvancePayments
                .Where(a => a.LaborId == laborId)
                .ToListAsync();
        }

        public async Task<AdvancePayment?> GetByIdAsync(int id)
        {
            await using var context = _contextFactory.CreateDbContext();

            return await context.AdvancePayments
                .Include(a => a.Labor)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(AdvancePayment payment)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));

            await using var context = _contextFactory.CreateDbContext();
            await context.AdvancePayments.AddAsync(payment);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AdvancePayment payment)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));

            await using var context = _contextFactory.CreateDbContext();
            context.AdvancePayments.Update(payment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var context = _contextFactory.CreateDbContext();

            var entity = await context.AdvancePayments.FindAsync(id);
            if (entity != null)
            {
                context.AdvancePayments.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// दिलेल्या LaborId साठी date range मध्ये किती advance दिले ते काढा
        /// </summary>
        public async Task<decimal> GetAdvanceByLaborInRangeAsync(int laborId, DateTime from, DateTime to)
        {
            await using var context = _contextFactory.CreateDbContext();

            return await context.AdvancePayments
                .Where(a => a.LaborId == laborId && a.DateGiven >= from && a.DateGiven <= to)
                .SumAsync(a => (decimal?)a.Amount) ?? 0m;
        }
        public async Task<List<AdvancePayment>> GetAdvancesBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.AdvancePayments
                .Where(a => a.DateGiven >= startDate && a.DateGiven <= endDate)
                .ToListAsync();
        }
       

    }
}
