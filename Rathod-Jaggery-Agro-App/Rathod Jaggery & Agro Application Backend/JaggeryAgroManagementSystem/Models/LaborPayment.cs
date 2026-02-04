namespace JaggeryAgroManagementSystem.Models
{
    public class LaborPayment
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public Labor Labor { get; set; }

        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMode { get; set; } // e.g., Cash, UPI, Bank
    }

}
