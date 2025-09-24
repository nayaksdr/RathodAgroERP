using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public interface IProduceService
    {
        Task<IEnumerable<JaggeryProduce>> ListAsync(DateTime? from = null, DateTime? to = null);
        Task<JaggeryProduce> GetAsync(int id);
        Task<(bool Success, string Error)> CreateAsync(JaggeryProduce produce);
        Task<(bool Success, string Error)> UpdateAsync(JaggeryProduce produce);
        Task DeleteAsync(int id);
        string GenerateBatchNumber(DateTime date);
    }
}
