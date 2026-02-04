namespace JaggeryAgroManagementSystem.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public DateTime PurchaseDate { get; set; }
        public double QuantityInKg { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal TotalAmount => (decimal)QuantityInKg * RatePerKg;
    }

}
