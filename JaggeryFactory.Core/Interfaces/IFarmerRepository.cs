using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IFarmerRepository
    {
        Task<Farmer> GetByIdAsync(int id);
        Task<IEnumerable<Farmer>> GetAllAsync();
        Task AddAsync(Farmer farmer);
        Task UpdateAsync(Farmer farmer);
        Task DeleteAsync(int id);
    }
}
