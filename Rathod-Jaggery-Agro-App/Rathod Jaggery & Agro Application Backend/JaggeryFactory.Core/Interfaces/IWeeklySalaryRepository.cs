using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    // JaggeryAgro.Core.Interfaces
    public interface IWeeklySalaryRepository
    {
        Task<IEnumerable<WeeklySalary>> GetAllAsync();
        Task<WeeklySalary?> GetByIdAsync(int id);
        Task AddAsync(WeeklySalary salary);
        Task DeleteAsync(int id);
        Task GenerateWeeklySalariesAsync(DateTime weekStart, DateTime weekEnd);
    }

}
