using Humanizer;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Data.Models;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class CanePaymentRepository : ICanePaymentRepository
    {
        private readonly ApplicationDbContext _context;
        

        public CanePaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CanePayment> AddAsync(CanePayment entity)
        {
            _context.CanePayments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public async Task<IEnumerable<CanePayment>> GetByFarmerAsync(int farmerId)
        => await _context.CanePayments.Include(p => p.Farmer)
        .Where(x => x.FarmerId == farmerId)
        .OrderByDescending(x => x.PaymentDate)
        .ToListAsync();


        public async Task<CanePayment?> GetAsync(int id)
        => await _context.CanePayments.Include(p => p.Farmer).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<CanePayment>> GetByFarmerAsyncNew(int farmerId)
        {
            return await _context.CanePayments
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
        }
        public async Task<IEnumerable<CanePaymentDTO>> GetByFarmerAsyncByName(int farmerId)
        {
            // Fetch payments for the farmer
            var paymentsQuery = from c in _context.CanePayments
                                where c.FarmerId == farmerId
                                select new CanePaymentDTO
                                {
                                    Id = c.Id,
                                    Amount = c.NetAmount,
                                    PaymentDate = c.PaymentDate,
                                    FarmerId = c.FarmerId,
                                    FarmerName = "" // temporary, will fill below
                                };

            var payments = await paymentsQuery.ToListAsync();

            // If no payments exist, create a default DTO
            if (!payments.Any())
            {
                var farmer = await _context.Farmers.FindAsync(farmerId);
                if (farmer != null)
                {
                    payments.Add(new CanePaymentDTO
                    {
                        Id = 0, // dummy id
                        Amount = 0,
                        PaymentDate = DateTime.MinValue,
                        FarmerId = farmer.Id,
                        FarmerName = farmer.Name
                    });
                }
            }
            else
            {
                // Fill FarmerName for existing payments
                var farmer = await _context.Farmers.FindAsync(farmerId);
                if (farmer != null)
                {
                    foreach (var p in payments)
                    {
                        p.FarmerName = farmer.Name;
                    }
                }
            }

            return payments;
        }



        public async Task<IEnumerable<CanePayment>> GetAllPaymentsAsync()
        {
            return await _context.CanePayments
                .Include(p => p.Farmer)
                .ToListAsync();
        }
        
    public async Task<CanePayment> GenerateFarmerPaymentAsync(int farmerId)
        {
            // Get farmer’s payments
            var payments = await _context.CanePayments
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();

            if (!payments.Any())
                return null; // Or throw exception / return empty record

            // Sum by type
            var totalAdvance = payments
                .Where(p => p.PaymentType == PaymentType.Advance)
                .Sum(p => p.NetAmount);

            var totalPaid = payments
                .Where(p => p.PaymentType == PaymentType.Final)
                .Sum(p => p.NetAmount);

            // Final Net Amount
            var netAmount = totalPaid - totalAdvance;

            // Return as a new CanePayment summary
            var result = new CanePayment
            {
                FarmerId = farmerId,
                NetAmount = netAmount,
                PaymentType = PaymentType.Final,
                PaymentDate = DateTime.Now
            };

            return result;
        }
        public async Task<CanePayment> ConfirmPaymentAsync(CanePayment payment)
        {
            _context.CanePayments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
        public async Task<CanePayment?> GetByIdAsync(int id)
        {
            return await _context.CanePayments
                .Include(p => p.Farmer) // Include related entity if needed
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<CanePayment>> GetAllWithFarmerAsync()
        {
            return await _context.CanePayments
                .Include(p => p.Farmer)
                .ToListAsync();
        }

        public async Task UpdateAsync(CanePayment payment)
        {
            _context.CanePayments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CanePayment>> GetAllAsync()
        {
            return await _context.CanePayments.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _context.CanePayments.FindAsync(id);
            if (payment != null)
            {
                _context.CanePayments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }   
    
}
