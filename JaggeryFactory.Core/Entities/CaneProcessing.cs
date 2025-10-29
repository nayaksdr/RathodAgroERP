using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class CaneProcessing
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public Labor Labor { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalTons { get; set; }
    }

}
