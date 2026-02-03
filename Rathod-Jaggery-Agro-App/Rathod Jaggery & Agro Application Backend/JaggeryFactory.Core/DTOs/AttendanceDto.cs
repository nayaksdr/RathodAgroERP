using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }

    }
}
