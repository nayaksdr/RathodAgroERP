using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;

namespace JaggeryAgro.Core.ViewModel
{
    public class JaggerySaleFilterViewModel
    {
        // Filters
        public string SearchDealer { get; set; }
        public int? SearchMemberId { get; set; }       // For PaidBy dropdown
        public string SearchPaymentMode { get; set; }  // Optional: Cash/UPI/Bank
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // Sales data
        public List<JaggerySale> Sales { get; set; }

        // Dropdown lists (optional, for strongly-typed binding)
        public List<Dealer> Dealers { get; set; }
        public List<Member> Members { get; set; }      // PaidBy members
        public List<string> PaymentModes { get; set; } // Cash, UPI, Bank
    }
}
