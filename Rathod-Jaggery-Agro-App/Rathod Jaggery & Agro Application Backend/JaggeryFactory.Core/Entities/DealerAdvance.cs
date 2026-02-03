using System;

namespace JaggeryAgro.Core.Entities
{
    public class DealerAdvance
    {
        public int Id { get; set; }

        // Foreign Key for Dealer
        public int DealerId { get; set; }
        // Navigation Property - EF uses this to fetch the Name
        public virtual Dealer Dealer { get; set; }

        // Foreign Key for the person who paid
        public int? PaidById { get; set; }
        // Navigation Property - EF uses this to fetch Employee info
        public virtual Member? PaidBy { get; set; }

        public string? PaymentMode { get; set; } // Cash, UPI, Bank
        public string? ProofImage { get; set; }  // Path of uploaded image
        public decimal Amount { get; set; }
        public DateTime AdvanceDate { get; set; } = DateTime.Now;
        public string? Note { get; set; }

        // REMOVED: public string DealerName { get; set; } 
        // Logic: You get the name via 'Dealer.Name'. 
        // Adding it here would try to create a new column in the DB.
    }
}