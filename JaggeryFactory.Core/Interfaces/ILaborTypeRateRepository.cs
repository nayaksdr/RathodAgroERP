using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ILaborTypeRateRepository
    {
        Task<IEnumerable<LaborTypeRate>> GetAllRatesAsync();
        // In ILaborTypeRateRepository
        Task<LaborTypeRate> GetCurrentRateByLaborTypeIdAsync(int laborTypeId);
        Task AddRateAsync(LaborTypeRate rate);
        Task<LaborTypeRate> GetByIdAsync(int id);
        Task UpdateRateAsync(LaborTypeRate rate);
        Task DeleteRateAsync(int id);       
        Task<decimal> GetPerTonRateAsync(int laborTypeId);
        Task<decimal> GetPerProductionRateAsync(int laborTypeId);

    }
}
