using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IWeeklyPaymentRepository
    {
        Task<IEnumerable<WeeklyPayment>> GetAllAsync();
        Task<WeeklyPayment?> GetByIdAsync(int id);
        Task GenerateWeeklyPaymentsAsync(DateTime weekStart, DateTime weekEnd);
        Task AddAsync(WeeklyPayment payment);
        Task DeleteAsync(int id);
    }
}
