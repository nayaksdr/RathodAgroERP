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
    public class LaborPaymentRepository : ILaborPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdvancePaymentRepository _advancePayRepo;

        // Use the interface here, not concrete class
        public LaborPaymentRepository(ApplicationDbContext context, IAdvancePaymentRepository advancePayRepo)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _advancePayRepo = advancePayRepo ?? throw new ArgumentNullException(nameof(advancePayRepo));
        }

        public async Task<bool> ExistsAsync(int laborId, DateTime? from, DateTime? to)
        {
            var query = _context.LaborPayments.AsQueryable();

            query = query.Where(p => p.LaborId == laborId);

            if (from.HasValue)
                query = query.Where(p => p.PaymentDate >= from.Value);

            if (to.HasValue)
                query = query.Where(p => p.PaymentDate <= to.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(LaborPayment payment)
        {
            _context.LaborPayments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LaborPayment>> GetAllAsync()
        {
            return await _context.LaborPayments.ToListAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
