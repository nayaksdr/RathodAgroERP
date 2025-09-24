using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class ProduceService : IProduceService
    {
        private readonly IProduceRepository _repo;

        public ProduceService(IProduceRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool Success, string Error)> CreateAsync(JaggeryProduce produce)
        {
            // Basic validations
            if (produce.QuantityKg <= 0) return (false, "Quantity must be greater than zero.");
            if (produce.UnitPrice < 0) return (false, "Unit price cannot be negative.");

            // Auto-generate batch number if not supplied
            if (string.IsNullOrWhiteSpace(produce.BatchNumber))
                produce.BatchNumber = GenerateBatchNumber(produce.ProducedDate);

            // Ensure uniqueness
            if (await _repo.BatchNumberExistsAsync(produce.BatchNumber))
                return (false, $"Batch number {produce.BatchNumber} already exists.");

            // Snapshot total cost
            produce.TotalCostSnapshot = produce.QuantityKg * produce.UnitPrice;
            // Initialize stock = quantity (assuming newly produced goes to stock)
            produce.StockKg = produce.TotalCost;

            await _repo.AddAsync(produce);
            return (true, null);
        }

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<JaggeryProduce> GetAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<JaggeryProduce>> ListAsync(DateTime? from = null, DateTime? to = null)
            => await _repo.GetAllAsync(from, to);

        public async Task<(bool Success, string Error)> UpdateAsync(JaggeryProduce produce)
        {
            // Validate
            if (produce.QuantityKg <= 0) return (false, "Quantity must be greater than zero.");
            if (produce.UnitPrice < 0) return (false, "Unit price cannot be negative.");

            // Check batch uniqueness
            if (await _repo.BatchNumberExistsAsync(produce.BatchNumber, produce.Id))
                return (false, "Another record uses this batch number.");

            // Recompute snapshot if you want to update snapshot;
            produce.TotalCostSnapshot = produce.QuantityKg * produce.UnitPrice;

            // Business: when updating quantity, adjust stock accordingly (simple behavior)
            // For robust behavior, you'd compute used/issued amounts vs produced
            // Here we set Stock = Quantity (replace) — adjust as per your inventory model
            produce.StockKg = produce.TotalCost;

            await _repo.UpdateAsync(produce);
            return (true, null);
        }

        public string GenerateBatchNumber(DateTime date)
        {
            // Example: JG-YYYYMMDD-<random 4 digits>
            var prefix = "JG";
            var d = date.ToString("yyyyMMdd");
            var rand = new Random().Next(1000, 9999);
            return $"{prefix}-{d}-{rand}";
        }
    }
}

