using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Data.Models
{
    public class FarmersCaneReportDto
    {
        public int FarmerId { get; set; }
        public string? FarmerName { get; set; }
        public decimal TotalTons { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalAdvance { get; set; }
        public decimal Balance { get; set; }

       
    }

}
