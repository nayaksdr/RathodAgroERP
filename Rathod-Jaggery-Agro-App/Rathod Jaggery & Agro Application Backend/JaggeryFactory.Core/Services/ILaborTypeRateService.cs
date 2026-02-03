using JaggeryAgro.Core.DTOs;
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
        Task<List<LaborTypeRateDto>> GetAllRatesAsync();
        Task<LaborTypeRateDto> GetCurrentRateAsync(int laborTypeId);
        Task AddRateAsync(LaborTypeRateDto rate);

        Task<LaborTypeRateDto> GetByIdAsync(int id);
        Task UpdateRateAsync(LaborTypeRateDto rate);
        Task DeleteRateAsync(int id);

    }

}
