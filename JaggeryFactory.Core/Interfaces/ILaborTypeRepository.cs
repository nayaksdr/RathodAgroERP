using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{ 
    public interface ILaborTypeRepository
    {
        Task<IEnumerable<LaborType>> GetAllAsync();
        Task<LaborType> GetByIdAsync(int id);
        Task AddAsync(LaborType type);
        Task UpdateAsync(LaborType type);
        Task DeleteAsync(int id);     
        
        Task<List<LaborType>> GetAllLaborTypesAsync();


        //IEnumerable<object> GetAllLaborTypes();
    }
}

