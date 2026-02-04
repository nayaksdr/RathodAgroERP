namespace JaggeryAgroManagementSystem.Models
{
    public class Labor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public decimal WagePerDay { get; set; }

        public ICollection<LaborAttendance> Attendances { get; set; }
        public ICollection<LaborPayment> Payments { get; set; }
    }

}
