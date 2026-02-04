namespace JaggeryAgroManagementSystem.DTOs
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double QuantityKg { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
