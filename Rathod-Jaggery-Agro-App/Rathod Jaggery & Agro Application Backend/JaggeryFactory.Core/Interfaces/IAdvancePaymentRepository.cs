using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IAdvancePaymentRepository
    {
        Task<List<AdvancePayment>> GetAllAsync();
        Task<IEnumerable<AdvancePayment>> GetAdvanceByLaborAsync(int laborId);
        Task<AdvancePayment?> GetByIdAsync(int id);
        Task AddAsync(AdvancePayment payment);
        Task UpdateAsync(AdvancePayment payment);
        Task DeleteAsync(int id);

        // 🔹 For Salary Calculation
        Task<decimal> GetAdvanceByLaborInRangeAsync(int laborId, DateTime from, DateTime to);
        Task<List<AdvancePayment>> GetAdvancesBetweenDatesAsync(DateTime startDate, DateTime endDate);
    }
}
