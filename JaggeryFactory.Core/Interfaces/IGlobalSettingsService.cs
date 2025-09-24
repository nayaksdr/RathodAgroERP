using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IGlobalSettingsService
    {
        // Async versions
        Task<decimal> GetDailyRateAsync();
        Task UpdateDailyRateAsync(decimal rate);

    }
}
