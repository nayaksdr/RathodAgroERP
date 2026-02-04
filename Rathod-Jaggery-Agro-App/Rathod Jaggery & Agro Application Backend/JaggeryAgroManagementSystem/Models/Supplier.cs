namespace JaggeryAgroManagementSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }

}
