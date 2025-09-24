using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Entities
{
    public class Expense
    {
        public int Id { get; set; }       
       

        [Range(1, double.MaxValue, ErrorMessage = "रक्कम शून्याहून जास्त असावी")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "सदस्य निवडा")]
        public int PaidById { get; set; }              // Who paid  // Foreign key
        public DateTime Date { get; set; } = DateTime.Now;
        public List<int> SharedByIds { get; set; } = new(); // Members sharing this expense      


        // Navigation property
        [ValidateNever] 
        public Member PaidBy { get; set; }

        [Required(ErrorMessage = "कृपया खर्च प्रकार निवडा.")]
        public int? ExpenseTypeId { get; set; }
        public ExpenseType ExpenseType { get; set; } // Navigation property
       
        public int? SplitwisePaymentId { get; set; }  // <-- ensure this is in your model
        public SplitwisePayment? SplitwisePayment { get; set; }

    }

}
