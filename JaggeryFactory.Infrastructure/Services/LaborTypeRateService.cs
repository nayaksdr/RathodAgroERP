using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Services
{
    public class LaborTypeRateService : ILaborTypeRateService
    {
        private readonly ILaborTypeRateRepository _repository;

        public LaborTypeRateService(ILaborTypeRateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LaborTypeRate>> GetAllRatesAsync()
        {
            return await _repository.GetAllRatesAsync();
        }

        public async Task<LaborTypeRate> GetCurrentRateAsync(int laborTypeId)
        {
            return await _repository.GetCurrentRateByLaborTypeIdAsync(laborTypeId);
        }

        public async Task AddRateAsync(LaborTypeRate rate)
        {
            await _repository.AddRateAsync(rate);
        }

      
        public async Task<LaborTypeRate> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

     
        public async Task UpdateRateAsync(LaborTypeRate rate)
        {
            if (rate == null)
                throw new ArgumentNullException(nameof(rate));

            await _repository.UpdateRateAsync(rate);
        }

        
        public async Task DeleteRateAsync(int id)
        {
            await _repository.DeleteRateAsync(id);
        }
    }
}
