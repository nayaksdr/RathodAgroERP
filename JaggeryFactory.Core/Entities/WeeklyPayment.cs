using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    
        public class WeeklyPayment
        {
            public int Id { get; set; }

            [Required]
            public int LaborId { get; set; }

            [ForeignKey("LaborId")]
            public Labor? Labor { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime WeekStartDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime WeekEndDate { get; set; }            

            // Display-only fields (not necessarily stored in DB)
            [Display(Name = "Labor Name")]
            public string? LaborName { get; set; }

            [Display(Name = "Labor Type")]
            public string? LaborType { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Week Start")]
            public DateTime WeekStart { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Week End")]
            public DateTime WeekEnd { get; set; }

            [Display(Name = "Days Present")]
            public int DaysPresent { get; set; }

            [Display(Name = "Daily Rate")]
            [DataType(DataType.Currency)]
            public decimal DailyRate { get; set; }

            [Display(Name = "Advance Deducted")]
            [DataType(DataType.Currency)]
            public decimal AdvanceDeducted { get; set; }

            [Display(Name = "Net Salary")]
            [DataType(DataType.Currency)]
            public decimal NetSalary { get; set; }
             public  DateTime PaymentDate { get; set; }

             public string? PaymentMethod { get; set; }
             public bool IsPaid { get; set; }

       
    }
    }


