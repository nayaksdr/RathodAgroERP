using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{   
        public class Farmer
        {
            public int Id { get; set; }

            [Required, StringLength(150)]
            public string Name { get; set; }

            [StringLength(20)]
            public string? Mobile { get; set; }

            [StringLength(250)]
            public string? Address { get; set; }

            [StringLength(100)]
            public string? Village { get; set; }

            [Required(ErrorMessage = "Aadhaar Number is required")]
            public string? AadhaarNumber { get; set; }

            public bool IsActive { get; set; } = true;

            // Navigation
            public ICollection<CanePurchase> CanePurchases { get; set; } = new List<CanePurchase>();         
            public ICollection<CaneAdvance> Advances { get; set; } = new List<CaneAdvance>();
            public ICollection<CanePayment> Payments { get; set; } = new List<CanePayment>();
    }
}

