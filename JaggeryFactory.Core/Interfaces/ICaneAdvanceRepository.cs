using JaggeryAgro.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ICaneAdvanceRepository
    {
        Task<CaneAdvance> AddAsync(CaneAdvance entity);
        Task UpdateAsync(CaneAdvance entity);
        Task DeleteAsync(CaneAdvance advance);

        Task<CaneAdvance?> GetAsync(int id);
        Task<CaneAdvance?> GetByIdAsync(int id);
        Task<IEnumerable<CaneAdvance>> GetAllAsync();
        Task<IEnumerable<CaneAdvance>> GetByFarmerAsync(int farmerId);
        Task<IEnumerable<CaneAdvance>> GetOpenByFarmerAsync(int farmerId); // RemainingAmount > 0
        Task<List<CaneAdvance>> GetAdvancePayByFarmerAsync(int farmerId);

        Task<decimal> GetAdvanceByFarmerAsync(int farmerId);
        Task<Dictionary<int, decimal>> GetAllAdvancesAsync();
    }
}
