using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class DashboardViewModel
    {
        public List<Expense> Expenses { get; set; }
        public List<JaggerySaleShare> JaggeryShares { get; set; }
        public List<MemberSummaryViewModel> MemberSummary { get; set; }
        public List<JaggeryShareStatusViewModel> JaggeryStatusList { get; set; } // <-- strongly typed
    }
}
