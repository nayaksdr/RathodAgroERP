using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class AdvancePayment
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        
        public DateTime DateGiven { get; set; }

        public decimal Amount { get; set; }        

        public Labor? Labor { get; set; }
        public DateTime Date { get; set; }
    }

}
