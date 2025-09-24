using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IExpenseTypeRepository
    {
        Task<List<ExpenseType>> GetAllAsync();
        Task<ExpenseType> GetByIdAsync(int id);    
     
        Task AddAsync(ExpenseType expenseType);
        Task UpdateAsync(ExpenseType expenseType);
        Task DeleteAsync(int id);
    }
}
