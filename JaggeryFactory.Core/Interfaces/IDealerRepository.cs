using JaggeryAgro.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IDealerRepository
    {
        Task<IEnumerable<Dealer>> GetAllAsync(string search, int page, int pageSize);
        Task<IEnumerable<Dealer>> GetAllAsync();
        Task<int> GetCountAsync(string search);
       // Task<List<Dealer>> GetAllAsync();
        Task<Dealer> GetByIdAsync(int id);
        Task AddAsync(Dealer dealer);
        Task UpdateAsync(Dealer dealer);
        Task DeleteAsync(int id);
      
    }

}
