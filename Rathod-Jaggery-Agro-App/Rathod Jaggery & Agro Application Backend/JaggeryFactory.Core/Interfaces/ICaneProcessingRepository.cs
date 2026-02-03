using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ICaneProcessingRepository
    {
        /// <summary>
        /// Calculate total cane tons processed by a specific labor within a given date range.
        /// </summary>
        /// <param name="laborId">ID of the labor</param>
        /// <param name="from">Start date</param>
        /// <param name="to">End date</param>
        /// <returns>Total tons processed</returns>
        
        /// <summary>
        /// Fetch all cane processing records within a date range, including labor details.
        /// </summary>
        /// <param name="from">Start date</param>
        /// <param name="to">End date</param>
        /// <returns>List of CaneProcessing records</returns>
        Task<IEnumerable<CaneProcessing>> GetByDateRangeAsync(DateTime from, DateTime to);

        /// <summary>
        /// Add a new CaneProcessing record.
        /// </summary>
        /// <param name="entity">CaneProcessing entity</param>
        Task AddAsync(CaneProcessing entity);

        /// <summary>
        /// Save all pending changes to the database.
        /// </summary>
        Task<decimal> GetTotalTonsByLaborInRangeAsync(int laborId, DateTime from, DateTime to);

        Task SaveAsync();
    }
}
