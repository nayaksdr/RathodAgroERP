using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ILaborPaymentRepository
    {
        Task<bool> ExistsAsync(int laborId, DateTime? from, DateTime? to);
        Task AddAsync(LaborPayment payment);
        Task<IEnumerable<LaborPayment>> GetAllAsync();
    }

}
