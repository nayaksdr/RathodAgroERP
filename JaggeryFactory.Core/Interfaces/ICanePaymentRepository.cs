using JaggeryAgro.Core.Entities;
using JaggeryAgro.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ICanePaymentRepository
    {
        Task<CanePayment> AddAsync(CanePayment entity);
        Task<IEnumerable<CanePayment>> GetByFarmerAsync(int farmerId);
        Task<CanePayment?> GetAsync(int id);
        Task<IEnumerable<CanePayment>> GetByFarmerAsyncNew(int farmerId);
        Task<IEnumerable<CanePaymentDTO>> GetByFarmerAsyncByName(int farmerId);        
        Task<IEnumerable<CanePayment>> GetAllPaymentsAsync();
        Task<CanePayment> GenerateFarmerPaymentAsync(int farmerId);
        Task<CanePayment> ConfirmPaymentAsync(CanePayment payment);
        Task<IEnumerable<CanePayment>> GetAllAsync();
        Task<CanePayment?> GetByIdAsync(int id);        
        Task UpdateAsync(CanePayment entity);
        Task DeleteAsync(int id);  // ✅ fixed
      
    }
}
