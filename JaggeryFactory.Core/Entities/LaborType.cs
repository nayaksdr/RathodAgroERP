using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class LaborType
    {
        public int Id { get; set; } // nullable { get; set; }

        [Required(ErrorMessage = "Labor Type Name is required")]
        public string? LaborTypeName { get; set; } // e.g., "Boiling Operator"

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
        public decimal DailyWage { get; set; }
        public ICollection<Labor> Labors { get; set; } = new List<Labor>();

        public virtual ICollection<LaborTypeRate> LaborTypeRates { get; set; }
    }

}
