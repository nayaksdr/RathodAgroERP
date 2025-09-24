using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Services
{
    public interface ILaborTypeRateService 
    {
        Task<IEnumerable<LaborTypeRate>> GetAllRatesAsync();
        Task<LaborTypeRate> GetCurrentRateAsync(int laborTypeId);
        Task AddRateAsync(LaborTypeRate rate);

        Task<LaborTypeRate> GetByIdAsync(int id);
        Task UpdateRateAsync(LaborTypeRate rate);
        Task DeleteRateAsync(int id);

    }

}
