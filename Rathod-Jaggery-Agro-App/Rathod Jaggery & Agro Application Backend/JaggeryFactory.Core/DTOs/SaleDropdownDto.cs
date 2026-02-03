using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    
        public class SaleDropdownDto
        {
            public int Id { get; set; }
            public string Label { get; set; } = string.Empty;
            public decimal TotalAmount { get; set; }
            public string SaleDate { get; set; } = string.Empty;
        }

        public class MemberDropdownDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }


