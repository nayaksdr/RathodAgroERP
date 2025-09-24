using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ICanePurchaseRepository : IGenericRepository<CanePurchase>
    {
        Task<CanePurchase> GetByIdAsync(int id);
        Task<IEnumerable<CanePurchase>> GetAllAsync(DateTime? from = null, DateTime? to = null, int? farmerId = null);
        Task AddAsync(CanePurchase purchase);
        Task UpdateAsync(CanePurchase purchase);
        Task DeleteAsync(int id);
        Task<IEnumerable<CanePurchase>> GetByFarmerAsync(int farmerId);
        Task<IEnumerable<CanePurchase>> GetByFarmerAsyncNew(int farmerId);      
        Task<int> SaveChangesAsync();
        Task<IEnumerable<CanePurchase>> GetByFarmerBetweenAsync(int farmerId, DateTime from, DateTime to);

    }
   
}
