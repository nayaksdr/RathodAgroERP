using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Services
{
    public interface IDealerAccountService
    {
        Task<decimal> GetAdvanceBalanceAsync(int dealerId, DateTime uptoDate);
    }
}
