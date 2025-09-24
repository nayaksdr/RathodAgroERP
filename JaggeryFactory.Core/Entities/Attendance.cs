using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Labor is required.")]
        public int LaborId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
       
        public Labor? Labor { get; set; }  // ✅ Required for .Include to work
     
        public List<SelectListItem>? Labors { get; set; }
    
    }

}
