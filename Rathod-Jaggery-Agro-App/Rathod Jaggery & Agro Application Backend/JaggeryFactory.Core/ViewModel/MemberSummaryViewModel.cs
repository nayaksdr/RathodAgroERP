using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class MemberSummaryViewModel
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public decimal SplitwisePaid { get; set; }   // Actual spent in splitwise
        public decimal JaggeryEarned { get; set; }   // Earnings from jaggery sale
        public decimal NetBalance { get; set; }      // Earned - Paid
        public object PieChartData { get; set; }     // { Paid, Earned } for chart
        
                  
    }

}
