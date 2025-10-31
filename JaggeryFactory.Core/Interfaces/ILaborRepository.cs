using JaggeryAgro.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ILaborRepository
    {
        // Async methods
        Task<List<Labor>> GetAllLaborsAsync(int? laborTypeId = null);
        Task<IEnumerable<Labor>> GetAllAsync();
        Task<List<Labor>> GetLaborsByTypeAsync(int laborTypeId);
        Task<Labor> GetLaborsByTypeIdAsync(int laborTypeId);
        Task<Labor> GetByIdAsync(int id);
        Task AddAsync(Labor labor);
        Task UpdateAsync(Labor labor);
        Task DeleteAsync(int id);

        // Synchronous methods (optional, for legacy code)
        IEnumerable<Labor> GetAll();
        Labor GetById(int id);
        void Add(Labor labor);
        void Update(Labor labor);
        void Delete(int id);

        // Utilities
        IEnumerable<Labor> GetAllWithLaborType();
        void Save();
        Task<Labor> GetByMobileAsync(string mobile);        
        Task<List<Attendance>> GetAttendanceAsync(int laborId);        
        Task<List<WeeklySalary>> GetWeeklySalariesAsync(int laborId);


        Task<Labor> GetByIdWithLaborTypeAsync(int id);
        Task<List<Labor>> GetAllLaborsWithLaborTypeAsync();
        Task<List<Labor>> GetLaborsByLaborTypeIdAsync(int laborTypeId);

    }
}
