using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class AdvancePaymentRepository : IAdvancePaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public AdvancePaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdvancePayment>> GetAllAsync()
        {
            return await _context.AdvancePayments
                .Include(a => a.Labor)
                    .ThenInclude(l => l.LaborType)   // 🔥 IMPORTANT
                .OrderByDescending(a => a.DateGiven)
                .ToListAsync();
        }


        public async Task<IEnumerable<AdvancePayment>> GetAdvanceByLaborAsync(int laborId)
        {
            return await _context.AdvancePayments
                .Where(a => a.LaborId == laborId)
                .ToListAsync();
        }

        public async Task<AdvancePayment?> GetByIdAsync(int id)
        {
            return await _context.AdvancePayments
               .Include(a => a.Labor)
                   .ThenInclude(l => l.LaborType)
               .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(AdvancePayment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            await _context.AdvancePayments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AdvancePayment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            _context.AdvancePayments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.AdvancePayments.FindAsync(id);
            if (entity != null)
            {
                _context.AdvancePayments.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// दिलेल्या LaborId साठी date range मध्ये किती advance दिले ते काढा
        /// </summary>
        public async Task<decimal> GetAdvanceByLaborInRangeAsync(
            int laborId,
            DateTime from,
            DateTime to)
        {
            return await _context.AdvancePayments
                .Where(a => a.LaborId == laborId &&
                            a.DateGiven >= from &&
                            a.DateGiven <= to)
                .SumAsync(a => (decimal?)a.Amount) ?? 0m;
        }

        public async Task<List<AdvancePayment>> GetAdvancesBetweenDatesAsync(
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.AdvancePayments
                .Where(a => a.DateGiven >= startDate &&
                            a.DateGiven <= endDate)
                .ToListAsync();
        }
    }
}
