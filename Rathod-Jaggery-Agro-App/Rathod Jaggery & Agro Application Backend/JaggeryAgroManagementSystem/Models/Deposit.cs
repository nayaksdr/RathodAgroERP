namespace JaggeryAgroManagementSystem.Models
{
    public class Deposit
    {
        public int Id { get; set; }

        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int? LaborId { get; set; }
        public Labor Labor { get; set; }

        public decimal Amount { get; set; }
        public DateTime DepositDate { get; set; }
        public string Remarks { get; set; }
    }

}
