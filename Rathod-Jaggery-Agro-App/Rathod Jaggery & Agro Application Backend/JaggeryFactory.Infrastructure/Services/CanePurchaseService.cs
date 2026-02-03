using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Services
{
    public class CanePurchaseService : ICanePurchaseService
    {
        private readonly ICanePurchaseRepository _repo;
        public CanePurchaseService(ICanePurchaseRepository repo) => _repo = repo;

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public async Task<CanePurchase> GetAsync(int id) => await _repo.GetByIdAsync(id);

        public Task<IEnumerable<CanePurchase>> ListAsync(DateTime? from = null, DateTime? to = null, int? farmerId = null)
            => _repo.GetAllAsync(from, to, farmerId);

        //public async Task<(bool Success, string Error)> CreateAsync(CanePurchase purchase)
        //{
        //    if (purchase.QuantityTon <= 0) return (false, "Quantity must be greater than 0");
        //    if (purchase.RatePerTon < 0) return (false, "Rate cannot be negative");
        //    // snapshot
        //    purchase.TotalAmountSnapshot = purchase.QuantityTon * purchase.RatePerTon;
        //    await _repo.AddAsync(purchase);
        //    return (true, null);
        //}

        public Task UpdateAsync(CanePurchase purchase)
        {
            purchase.TotalAmountSnapshot = purchase.QuantityTon * purchase.RatePerTon;
            return _repo.UpdateAsync(purchase);
        }
        public async Task<(bool, string)> CreateAsync(CanePurchase purchase)
        {
            if (purchase == null) return (false, "Invalid purchase data");

            // Optional double-check for safety
            purchase.TotalAmountSnapshot = purchase.QuantityTon * purchase.RatePerTon;

            await _repo.AddAsync(purchase);
            await _repo.SaveChangesAsync();
            return (true, null);
        }

    }
}
