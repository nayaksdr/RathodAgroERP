using JaggeryAgro.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ICaneAdvanceRepository
    {
        // 🔹 Basic CRUD operations
        Task<CaneAdvance> AddAsync(CaneAdvance entity);
        Task UpdateAsync(CaneAdvance entity);
        Task DeleteAsync(CaneAdvance advance);

        // 🔹 Fetch single record
        Task<CaneAdvance?> GetAsync(int id);
        Task<CaneAdvance?> GetByIdAsync(int id);

        // 🔹 Fetch all or filtered lists
        Task<IEnumerable<CaneAdvance>> GetAllAsync();
        Task<IEnumerable<CaneAdvance>> GetByFarmerAsync(int farmerId);
        Task<IEnumerable<CaneAdvance>> GetOpenByFarmerAsync(int farmerId); // RemainingAmount > 0

        // 🔹 Simplified / summary data
        Task<List<CaneAdvance>> GetAdvancePayByFarmerAsync(int farmerId);
        Task<decimal> GetAdvanceByFarmerAsync(int farmerId);
        Task<Dictionary<int, decimal>> GetAllAdvancesAsync();
    }
}
