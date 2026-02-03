namespace JaggeryAgro.Core.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public decimal Amount { get; set; }

        // The actual payment date
        public DateTime PaidOn { get; set; }

        // New fields for weekly payment tracking
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }

        // When the salary was generated (can be different from PaidOn)
        public DateTime GeneratedDate { get; set; }

        public Labor? Labor { get; set; }
        public bool IsPaid { get; set; }
        
    }
}
