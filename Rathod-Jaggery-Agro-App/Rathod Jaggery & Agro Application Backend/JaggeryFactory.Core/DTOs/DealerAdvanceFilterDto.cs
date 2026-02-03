using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class DealerAdvanceFilterDto
    {
        public int? DealerId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
