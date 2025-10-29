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
    public class JaggerySaleRepository : IJaggerySaleRepository
    {
        private readonly ApplicationDbContext _context;

        public JaggerySaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all sales
        public async Task<List<JaggerySale>> GetAllAsync()
        {
            return await _context.JaggerySales
                                 .OrderByDescending(s => s.SaleDate)
                                 .ToListAsync();
        }

        // Get all sales including Dealer
        public async Task<List<JaggerySale>> GetAllIncludingDealerAsync()
        {
            return await _context.JaggerySales
                                 .Include(s => s.Dealer)
                                 .OrderByDescending(s => s.SaleDate)
                                 .ToListAsync();
        }

        // Get filtered sales by dealer name and/or dates
        public async Task<List<JaggerySale>> GetFilteredAsync(string searchDealer, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.JaggerySales
                                .Include(s => s.Dealer)   // Include Dealer
                                .Include(s => s.PaidBy)   // Include PaidBy (member)
                                .Include(s => s.Labor)    // ✅ Include Labor
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchDealer))
                query = query.Where(s => s.Dealer.Name.Contains(searchDealer));

            if (fromDate.HasValue)
                query = query.Where(s => s.SaleDate >= fromDate.Value.Date);

            if (toDate.HasValue)
                query = query.Where(s => s.SaleDate <= toDate.Value.Date);

            query = query.Where(s => s.PaidBy != null);

            return await query
                         .OrderByDescending(s => s.SaleDate)
                         .ToListAsync();
        }


        // Get a sale by ID
        public async Task<JaggerySale> GetByIdAsync(int id)
        {
            return await _context.JaggerySales
                                 .Include(s => s.Dealer)
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Add a new sale
        public async Task AddAsync(JaggerySale sale)
        {
            await _context.JaggerySales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        // Update an existing sale
        public async Task UpdateAsync(JaggerySale sale)
        {
            _context.JaggerySales.Update(sale);
            await _context.SaveChangesAsync();
        }

        // Delete a sale
        public async Task DeleteAsync(int id)
        {
            var sale = await _context.JaggerySales.FindAsync(id);
            if (sale != null)
            {
                _context.JaggerySales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }

        // Get all sales for a specific dealer
        public async Task<List<JaggerySale>> GetByDealerAsync(int dealerId)
        {
            return await _context.JaggerySales
                                 .Include(s => s.Dealer)
                                 .Where(s => s.DealerId == dealerId)
                                 .OrderByDescending(s => s.SaleDate)
                                 .ToListAsync();
        }

        public async Task<List<JaggerySale>> QueryAsync(int? dealerId, DateTime? from, DateTime? to)
        {
            var query = _context.JaggerySales
                                .Include(s => s.Dealer)
                                .AsQueryable();

            if (dealerId.HasValue)
                query = query.Where(s => s.DealerId == dealerId.Value);

            if (from.HasValue)
                query = query.Where(s => s.SaleDate >= from.Value.Date);

            if (to.HasValue)
                query = query.Where(s => s.SaleDate <= to.Value.Date);

            return await query.OrderByDescending(s => s.SaleDate).ToListAsync();
        }
        //public async Task<decimal> GetTotalAdvanceAppliedByDealerExceptAsync(int dealerId, DateTime uptoDate, int excludeSaleId)
        //{
        //    return await _context.JaggerySales
        //        .Where(s => s.DealerId == dealerId && s.SaleDate <= uptoDate && s.Id != excludeSaleId)
        //        .SumAsync(s => (decimal?)s.AdvancePaid ?? 0);
        //}

        public async Task<decimal> GetTotalAdvanceAppliedByDealerAsync(int dealerId, DateTime? uptoDate = null)
        {
            var query = _context.JaggerySales
                .Where(s => s.DealerId == dealerId);

            if (uptoDate.HasValue)
                query = query.Where(s => s.SaleDate <= uptoDate.Value);

            return await query.SumAsync(s => (decimal?)s.AdvancePaid ?? 0);
        }
        public async Task<List<JaggerySaleShare>> GetAllSharesAsync()
        {
            return await _context.JaggerySaleShares
                .Include(s => s.Member)
                .Include(s => s.JaggerySale)
                    .ThenInclude(j => j.Dealer)
                .ToListAsync();
        }

        public async Task<List<JaggerySalePayment>> GetAllPaymentsAsync()
        {
            return await _context.JaggerySalePayments
                .Include(p => p.FromMember)
                .Include(p => p.ToMember)
                .Include(p => p.JaggerySale)
                .ToListAsync();
        }

        public async Task AddShareAsync(JaggerySaleShare share)
        {
            _context.JaggerySaleShares.Add(share);
            await _context.SaveChangesAsync();
        }

        public async Task AddPaymentAsync(JaggerySalePayment payment)
        {
            _context.JaggerySalePayments.Add(payment);
            await _context.SaveChangesAsync();
        }
        public async Task<decimal> GetTotalAdvanceAppliedByDealerAsync(int dealerId, DateTime uptoDate)
        {
            return await _context.JaggerySales
                .Where(s => s.DealerId == dealerId && s.SaleDate <= uptoDate)
                .SumAsync(s => (decimal?)s.AdvancePaid) ?? 0;
        }

        public async Task<decimal> GetTotalAdvanceAppliedByDealer(int dealerId)
        {
            return await _context.JaggerySales
                .Where(s => s.DealerId == dealerId)
                .SumAsync(s => (decimal?)s.AdvancePaid) ?? 0;
        }

        public async Task<decimal> GetTotalAdvanceAppliedByDealerExceptAsync(int dealerId, int excludeSaleId)
        {
            return await _context.JaggerySales
                .Where(s => s.DealerId == dealerId && s.Id != excludeSaleId)
                .SumAsync(s => (decimal?)s.AdvancePaid) ?? 0;
        }
<<<<<<< HEAD
        public async Task<decimal> GetTotalProductionByLaborInRangeAsync(int laborId, DateTime from, DateTime to)
        {
            if (laborId <= 0)
                throw new ArgumentException("Invalid LaborId provided.", nameof(laborId));

            // Using nullable sum to safely handle empty results
            var totalProduction = await _context.JaggerySales
                .AsNoTracking()
                .Where(x => x.LaborId == laborId && x.SaleDate >= from && x.SaleDate <= to)
                .SumAsync(x => (decimal?)x.QuantityInKg) ?? 0m;

            return totalProduction;
        }
=======
>>>>>>> 33a9ded78b728faf46d40805babd453ca661cb61
    }
}

