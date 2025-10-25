using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IJaggeryProductionRepository
    {
        /// <summary>
        /// Calculate total jaggery production by a specific labor within a given date range.
        /// </summary>
        /// <param name="laborId">ID of the labor</param>
        /// <param name="from">Start date</param>
        /// <param name="to">End date</param>
        /// <returns>Total quantity produced</returns>
        Task<decimal> GetTotalProductionByLaborInRangeAsync(int laborId, DateTime from, DateTime to);

        /// <summary>
        /// Fetch all jaggery production records within a date range, including labor details.
        /// </summary>
        /// <param name="from">Start date</param>
        /// <param name="to">End date</param>
        /// <returns>List of JaggeryProduction records</returns>
        Task<IEnumerable<JaggeryProduction>> GetByDateRangeAsync(DateTime from, DateTime to);

        /// <summary>
        /// Add a new JaggeryProduction record.
        /// </summary>
        /// <param name="entity">JaggeryProduction entity</param>
        Task AddAsync(JaggeryProduction entity);

        /// <summary>
        /// Save all pending changes to the database.
        /// </summary>
        Task SaveAsync();
    }
}
