using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class DailyJaggeryShareDto
    {
        public string Date { get; set; }
        public decimal TotalShare { get; set; }  // Total assigned share
        public decimal Paid { get; set; }        // Total paid (including payments between members)
        public decimal Pending { get; set; }     // Total pending

        public DailyJaggeryShareDto(string date, decimal totalShare, decimal paid, decimal pending)
        {
            Date = date;
            TotalShare = totalShare;
            Paid = paid;
            Pending = pending;
        }
    }

}
