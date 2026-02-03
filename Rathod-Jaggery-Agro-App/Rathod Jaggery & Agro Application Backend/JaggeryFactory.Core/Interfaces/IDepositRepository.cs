using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IDepositRepository
    {
        Task<IEnumerable<Deposit>> GetAllAsync();
        Task<Deposit?> GetByIdAsync(int id);
        Task<Deposit> AddAsync(Deposit deposit);
        Task UpdateAsync(Deposit deposit);
        Task DeleteAsync(int id);
    }
}
