using JaggeryAgro.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Services
{
    public interface IDashboardService
    {
        Task<DashboardDailyVm> GetDailyAsync(int days = 30, CancellationToken ct = default);
    }
}
