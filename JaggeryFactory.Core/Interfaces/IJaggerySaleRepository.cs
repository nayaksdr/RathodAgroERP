using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IJaggerySaleRepository
    {
        // Get all sales with optional dealer and date filters
        Task<List<JaggerySale>> GetFilteredAsync(string dealerId, DateTime? fromDate, DateTime? toDate);

        //Task<decimal> GetTotalAdvanceAppliedByDealerExceptAsync(int dealerId, DateTime uptoDate, int excludeSaleId);

        // Get all sales for a specific dealer
        Task<List<JaggerySale>> GetByDealerAsync(int dealerId);

        // Flexible query with nullable filters
        Task<List<JaggerySale>> QueryAsync(int? dealerId, DateTime? from, DateTime? to);

        // Get sale by Id
        Task<JaggerySale> GetByIdAsync(int id);
        //public Task<decimal> GetTotalAdvanceAppliedByDealerAsync(int dealerId, DateTime? uptoDate = null);


        // Add a new sale
        Task AddAsync(JaggerySale sale);

        // Update an existing sale
        Task UpdateAsync(JaggerySale sale);

        // Delete a sale by Id
        Task DeleteAsync(int id);
        Task<List<JaggerySale>> GetAllAsync();
        Task<List<JaggerySale>> GetAllIncludingDealerAsync();
        Task<List<JaggerySaleShare>> GetAllSharesAsync();

        // Get all Jaggery Sale Payments with related Member and Sale info
        Task<List<JaggerySalePayment>> GetAllPaymentsAsync();

        // Optional: Add a JaggerySaleShare
        Task AddShareAsync(JaggerySaleShare share);

        // Optional: Add a JaggerySalePayment
        Task AddPaymentAsync(JaggerySalePayment payment);

        Task<decimal> GetTotalAdvanceAppliedByDealerAsync(int dealerId, DateTime uptoDate);

        Task<decimal> GetTotalAdvanceAppliedByDealerExceptAsync(int dealerId, int excludeSaleId);
        Task<decimal> GetTotalProductionByLaborInRangeAsync(int laborId, DateTime from, DateTime to);


    }
}
