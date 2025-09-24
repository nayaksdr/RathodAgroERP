using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IDealerAdvanceRepository
    {
        Task<DealerAdvance> GetByIdAsync(int id);
        Task AddAsync(DealerAdvance entity);
        Task UpdateAsync(DealerAdvance entity);
        Task DeleteAsync(int id);

        Task<IEnumerable<DealerAdvance>> GetAllAsync();
        Task<IEnumerable<DealerAdvance>> FilterAsync(int? dealerId, DateTime? from, DateTime? to);

        /// <summary>
        /// Total advances for a dealer (optionally up to a date).
        /// </summary>
        Task<decimal> GetTotalAdvanceByDealerAsync(int dealerId, DateTime? uptoDate = null);
        Task<decimal> GetTotalAdvanceByDealer(int dealerId);
    }
}
