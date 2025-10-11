using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class JaggeryShareStatusViewModel
    {
        public int ShareId { get; set; }
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public string DealerName { get; set; }
        public string MemberName { get; set; }
        public decimal ShareAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string Status { get; set; }
    }

}
