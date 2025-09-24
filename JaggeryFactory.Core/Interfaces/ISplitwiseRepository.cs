using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ISplitwiseRepository
    {
        // Members
        Task<List<Member>> GetMembersAsync();
        Task AddMemberAsync(Member member);

        // Expenses
        Task<List<Expense>> GetExpensesAsync();
        Task AddExpenseAsync(Expense expense);
        Task<Expense> GetExpenseByIdAsync(int id);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);
        Task<List<SplitwisePayment>> GetPaymentsAsync();
        Task AddPaymentAsync(SplitwisePayment payment);
        Task UpdateMemberBalanceAsync(int memberId, decimal newBalance);      

        // Splitwise calculations
        Task<Dictionary<int, decimal>> GetBalancesAsync();
        Task<List<(int from, int to, decimal amount)>> GetSettlementsAsync();
    }
}
