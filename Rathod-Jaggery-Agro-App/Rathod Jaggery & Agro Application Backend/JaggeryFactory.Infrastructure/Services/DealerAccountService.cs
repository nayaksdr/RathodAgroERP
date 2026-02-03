using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.Services;
using System;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Services
{
    public class DealerAccountService : IDealerAccountService
    {
        private readonly IDealerAdvanceRepository _advRepo;
        private readonly IJaggerySaleRepository _saleRepo;

        public DealerAccountService(
            IDealerAdvanceRepository advRepo,
            IJaggerySaleRepository saleRepo)
        {
            _advRepo = advRepo ?? throw new ArgumentNullException(nameof(advRepo));
            _saleRepo = saleRepo ?? throw new ArgumentNullException(nameof(saleRepo));
        }

        public async Task<decimal> GetAdvanceBalanceAsync(int dealerId, DateTime uptoDate)
        {
            var totalAdv = await _advRepo.GetTotalAdvanceByDealerAsync(dealerId, uptoDate);
            var totalApplied = await _saleRepo.GetTotalAdvanceAppliedByDealerAsync(dealerId, uptoDate);
            return totalAdv - totalApplied;
        }
    }
}
