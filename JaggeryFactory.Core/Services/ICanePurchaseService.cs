using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Services
{
    public interface ICanePurchaseService
    {
        Task<IEnumerable<CanePurchase>> ListAsync(DateTime? from = null, DateTime? to = null, int? farmerId = null);
        Task<CanePurchase> GetAsync(int id);
        Task<(bool Success, string Error)> CreateAsync(CanePurchase purchase);
        Task UpdateAsync(CanePurchase purchase);
        Task DeleteAsync(int id);
    }
}
