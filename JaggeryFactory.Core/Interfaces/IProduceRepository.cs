using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IProduceRepository
    {
        Task<JaggeryProduce> GetByIdAsync(int id);
        Task<IEnumerable<JaggeryProduce>> GetAllAsync(DateTime? from = null, DateTime? to = null);
        Task AddAsync(JaggeryProduce produce);
        Task UpdateAsync(JaggeryProduce produce);
        Task DeleteAsync(int id);
        Task<bool> BatchNumberExistsAsync(string batchNumber, int? exceptId = null);

        Task<decimal> GetTotalUnitPriceAsync();
        Task<decimal> GetTotalCostAsync();
    }
}
