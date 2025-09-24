using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Services
{
    public interface IFarmerService
    {
        Task<IEnumerable<Farmer>> ListAsync();
        Task<Farmer> GetAsync(int id);
        Task CreateAsync(Farmer farmer);
        Task UpdateAsync(Farmer farmer);
        Task DeleteAsync(int id);

    }
}
